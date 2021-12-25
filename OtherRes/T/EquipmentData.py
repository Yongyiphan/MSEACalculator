import pandas
import requests
import lxml
import cchardet
import time
from bs4 import BeautifulSoup

### PROGRAM FLOW ####
# MAIN PAGE -> GET EQUIPNAME/LINK 
# TRAVERSE TO EQUIPLINK PAGE -> GET WEAPON SET'S LINK
# TRAVERSE TO WEAPON'LINK -> RECORD INFORMATION
root = 'DefaultData'
defaulturl = "https://maplestory.fandom.com"
weaponUrl = "/wiki/Category:Weapons"
secUrl = "/wiki/Category:Secondary_Weapons"
equipSetUrl = "/wiki/Category:Equipment_Sets"
genesisWeapUrl = "/wiki/Sealed_Genesis_Weapon_Box"
superiorEquipUrl = "/wiki/Category:Superior_Equipment"
MainStat = ['STR','DEX','INT','LUK']
EquipSetTrack = [
    'Sengoku', 'Boss Accessory', 'Pitched Boss', 'Seven Days', "Ifia's Treasure",'Mystic','Ardentmill','Blazing Sun'
    ,'8th', 'Root Abyss','AbsoLab', 'Arcane'
    ,'Lionheart', 'Dragon Tail','Falcon Wing','Raven Horn','Shark Tooth'
    ,'Gold Parts','Pure Gold Parts'
    ]
WeapSetTrack = [
    'Utgard', 'Lapis','Lazuli'
    ,'Fafnir','AbsoLab', 'Arcane', 'Genesis'
    ]
ArmorSet = ['Hat','Top','Bottom','Overall','Shoes','Cape','Gloves']
Mclasses = ['Warrior','Knight', 'Bowman', 'Archer', 'Magician','Mage','Thief', 'Pirate']
##NAMING CONVENTION
#NAME, SET, TYPE, CLASSTYPE, SLOT
#LEVEL, MS, SS, AS, HP, MP, DEF, ATK, MATK
SetCol = ['EquipSet','SetEffect','MainStat', 'SecStat','AllStat','HP','MP','DEF','ATK','MATK','NDMG','IED','BDMG','CDMG']
WeapCol = ['WeaponSet','WeaponType','Level','AtkSpd','MainStat','SecStat','HP','DEF','ATK','MATK','SPD','BDMG','IED']
ArmorCol = ['EquipSet','ClassType','EquipSlot','EquipLevel','MainStat','SecStat','AllStat', 'HP','MP','DEF','ATK','MATK','SPD','JUMP','IED']
AccCol = ['EquipName','EquipSet','ClassType','EquipSlot','EquipLevel','MainStat','SecStat','AllStat','HP','MP','DEF','ATK','MATK','SPD','JUMP','IED']
trackMinLevel = 140

def main():

    

    ###WEAPON ==> GOTO LINK ==> GOTO HEADER LINK
    ##ITERATE TABLE return DF

    start = time.time()
    request_session = requests.session()

    WeaponDF = pandas.DataFrame()
    ArmorDF = pandas.DataFrame()
    AccessoriesDF = pandas.DataFrame()
    SetEffectDF = pandas.DataFrame()
    GenesisDF = pandas.DataFrame()
    WeaptoJobDF =  pandas.DataFrame()

    ##PROCESS
    ###EQUIPMENT_SETS ==> GATHER TRACKSET LINKS ###
    ##ITERATE TRACKSET LINKS
    ## CREATE 2 DATAFRAME
    ## 1: EQUIPMENT SET EFFECTS
    ## 2: EQUIPMENT ARMOR/ACCESSORIES
    SetEffectDF, ArmorDF, AccessoriesDF = retrieveEquipmentSet(request_session)
    
    ##RETRIEVE TYRANT
    tAcc, tArmor = retrieveTyrant(request_session)


    SetEffectDF = cleanSetEffectDF(SetEffectDF)
    
    WeaptoJobDF, WeaponDF = retrieveWeapDF(request_session)
    WeaponDF = cleanWeapDF(WeaponDF)
    
    WeaptoJobDF = cleanWTJDF(WeaptoJobDF)
    
    ArmorDF = ArmorDF.append(tArmor, ignore_index=True)
    ArmorDF = ArmorDF[ArmorCol]
    ArmorDF = ArmorDF.fillna(0)
    ##MANUALLY ADDING TYRANY



    AccessoriesDF = AccessoriesDF.append(tAcc, ignore_index=True)
    AccessoriesDF = AccessoriesDF[AccCol]
    AccessoriesDF = AccessoriesDF.fillna(0)
    
    SecWeapDF = retrieveSecWeap(request_session)
    SecWeapDF = SecWeapDF.fillna(0)
    
    SetEffectDF.to_csv('DefaultData\\SetEffectData.csv')
    WeaponDF.to_csv('DefaultData\\WeaponData.csv')
    ArmorDF.to_csv('DefaultData\\ArmorData.csv')
    AccessoriesDF.to_csv('DefaultData\\AccessoriesData.csv')
    SecWeapDF.to_csv('DefaultData\\SecondaryWeapData.csv')
    WeaptoJobDF.to_csv('DefaultData\\ClassWeapData.csv')
    
    end = time.time()

    print(f"Total time taken is {end-start}")
    return



def retrieveEquipmentSet(session):

    equipSetLinkListPage = session.get(defaulturl + equipSetUrl)
    soup = BeautifulSoup(equipSetLinkListPage.content, 'lxml')
    linkLists = soup.find_all('a', class_='category-page__member-link')

    ignoreSetKeyWords = ['Immortal', 'Eternal','Walker','Anniversary']

    ArmorDF = pandas.DataFrame()
    AccessoriesDF = pandas.DataFrame()
    SetEffectDF = pandas.DataFrame()

    for links in linkLists:
        linkText = links.next.lower()
        for li in EquipSetTrack:
            if li.lower() in linkText and any(ig.lower() in linkText for ig in ignoreSetKeyWords) == False:

                Mstart = time.time()
                
                tempCollection = retrieveContents(links['href'], session, li)
                
                SetEffectDF = SetEffectDF.append(tempCollection['SetEffect'], ignore_index=True) 
                ArmorDF = ArmorDF.append(tempCollection['Armor'], ignore_index=True)
                AccessoriesDF = AccessoriesDF.append(tempCollection['Accessories'], ignore_index=True)
                
                Mend = time.time()
                print(f"{li} equips added in {Mend - Mstart }. ")
                break

    return SetEffectDF, ArmorDF, AccessoriesDF

def retrieveContents(subUrl, session, equipSet):

    #SCRAPING EACH PAGE
    subPage = session.get(defaulturl + subUrl)
    if subPage.status_code != 200:
        return
    
    totalPageContent = BeautifulSoup(subPage.content, 'lxml')
    wikitables = totalPageContent.find_all('table', class_="wikitable")

    initETDF = pandas.DataFrame()

    smallCollection = {}
    smallCollection['SetEffect'] = retrieveSetEffect(wikitables[0], equipSet)
    smallCollection['Armor'], smallCollection['Accessories'] = retrieveEquips(wikitables[1], equipSet)

    
    return smallCollection

def retrieveSetEffect(wikitable, equipSet):

    #SCRAPING EACH TABLE
    start = time.time()
    currentDF = pandas.DataFrame()

    tdContent = wikitable.find_all('td')

    startCounter = len(tdContent) % 2

    for i in range(startCounter, len(tdContent), 2):
        SetData = {}
        SetData['EquipSet'] = equipSet
        SetData['SetEffect'] = tdContent[i].next.next.split(" ")[0]
        statList = tdContent[i].get_text(separator = '\n').split('\n')[1:]
        SetData.update(assignToDict(statList))
    
        currentDF = currentDF.append(SetData, ignore_index=True)
        
    currentDF = currentDF.fillna(0)
    
    return currentDF

def retrieveEquips(wikitable, equipSet):

    ArmorDF = pandas.DataFrame()
    AccDF = pandas.DataFrame()
    tdContent = wikitable.find_all('td')

    for i in range(0, len(tdContent), 4):
        EquipData = {}
        EquipData['EquipSet'] = removeN(equipSet,  ['\n'])
        equipName = tdContent[i].get_text()
        equipslot = removeN(tdContent[i+1].get_text(),'\n')
        EquipData['EquipSlot'] = equipslot.split(' ')[0] if equipslot.find('Pocket') != -1 else equipslot 
        equipType = 'Armor' if EquipData['EquipSlot'] in ArmorSet else 'Accessories'
        if equipType == 'Accessories':
            TequipName = removeN(equipName, ['\n', ":"])
            TequipName = TequipName.split(' ')
            if EquipData['EquipSlot'] in TequipName:
                TequipName.remove(EquipData['EquipSlot'])
            for mc in Mclasses:
                if mc in TequipName:
                    TequipName.remove(mc)
            
            TequipName = ' '.join(TequipName)
            EquipData['EquipName'] = TequipName
            
            
        
        requirements = tdContent[i+2].get_text(separator = '\n').split("\n")
        EquipData['EquipLevel'] = '0' if requirements[0].find('None') != -1 else requirements[0].split(" ")[-1]
        if len(requirements) > 2:
            EquipData['ClassType'] = requirements[1].split(" ")[-1]
        else:
            EquipData['ClassType'] = 'Any'

        
        effects =  tdContent[i+3].get_text(separator = '\n').split('\n')
        EquipData.update(assignToDict(effects))

        if equipType == 'Accessories':
            AccDF = AccDF.append(EquipData, ignore_index=True)
        else:
            ArmorDF = ArmorDF.append(EquipData, ignore_index=True)
        
    ArmorDF = ArmorDF.fillna(0)
    AccDF = AccDF.fillna(0)

    return ArmorDF, AccDF

def retrieveWeapDF(session):

    Page = session.get(defaulturl + weaponUrl)
    ignoreList = ['Secondary','Scepter']
    soup = BeautifulSoup(Page.content, 'lxml')
    weaponLinksList = soup.find_all('a', class_='category-page__member-link')
    WeapDF = pandas.DataFrame()
    ClassWeapD = {}
    LWeaponType = []
    LJobType = []
    tempLinkList = []
    for link in weaponLinksList:
        if any(link.next.lower().find(t.lower()) != -1 for t in ignoreList) == True:
            continue
        tempLinkList.append(link['href'])
        WTJ, currentDF = retrieveWeapContent(link['href'], session)
        WeapDF = WeapDF.append(currentDF, ignore_index=True)
        for weap in WTJ:
            for i in WTJ[weap]:
                LWeaponType.append(weap)
                LJobType.append(i)

    ClassWeapD['JobType'] =LJobType
    ClassWeapD['WeaponType'] = LWeaponType
    ClassWeapDF = pandas.DataFrame(ClassWeapD)

    # print(ClassWeapDF)
    
    # testSelection = 14
    # cfd = retrieveWeapContent(tempLinkList[testSelection], session)
    # print(cfd)
    return ClassWeapDF, WeapDF

def retrieveWeapContent(link, session):

    #Navigate to page with weapon in list
    start = time.time()
    weaponListLink = BeautifulSoup(session.get(defaulturl + link).content, 'lxml').find_all('div', class_='mw-parser-output')[0].find_all('a')[0]['href']

    WeaponPage = BeautifulSoup(session.get(defaulturl + weaponListLink).content,'lxml')
    titleContent = WeaponPage.find_all('h1',class_='page-header__title')[0]
    MainContent = WeaponPage.find_all('div', class_='mw-parser-output')[0]
    HeaderP = MainContent.find_all('p')[0]

    weaponType = removeN(titleContent.next, ['\n','\t'])
    weapJob = []
    WeapToJob = {}

    
    if any(i.name == 'a' for i in HeaderP.contents) == False:
        LI = MainContent.find_all('ul')[0].find_all('li')
        for li in LI:
            for i in li.contents:
                if i.name == 'a' and i.nextSibling.get_text().lower().find('(') != -1:
                    weapJob.append(i.next)
    
    else:
        if HeaderP.get_text().lower().find('exclusive') != -1:
            for t in HeaderP.contents:
                if t.previous.find('exclusive') != -1 and t.name == 'a':
                    weapJob.append(t.next)
                if t.next.next.find('conjunction') != -1:
                    break
        
        elif HeaderP.get_text().lower().find('primary') != -1:
            startRecord = False
            for t in HeaderP.contents:
                if t.get_text().lower().find('primary') != -1:
                    startRecord = True
                    continue
                if startRecord == True and t.name == 'a':
                    weapJob.append(t.next)
                if t.next.next.find('conjunction') != -1:
                    break
                if t.get_text().lower().find('(') != -1:
                    break
                

    WeapToJob[weaponType] = weapJob

    tableC = MainContent.find_all('table',class_='wikitable')[0].find_all('td')
    currentDF = pandas.DataFrame()
    for i in range(0, len(tableC),3):
        ItemData = {}
        if any(ele.lower() in tableC[i].get_text().lower() for ele in WeapSetTrack) == True:
            retrievedSet = tableC[i].get_text()[:-1].replace(weaponType, "")
            if retrievedSet.lower().find("sealed") != -1:
                continue     
            if retrievedSet.lower().find("utgard") != -1:
                ItemData["WeaponSet"] = "Fensalir"
            elif retrievedSet.lower().find("lapis") != -1 or retrievedSet.lower().find("lazuli") != -1:
                ItemData["WeaponSet"] = " ".join(retrievedSet.split(" ")[1:])
                if retrievedSet.lower().find("genesis") != -1:
                    ItemData["WeaponSet"] = 'Genesis'
            else:
                ItemData["WeaponSet"] = WeapSetTrack[returnIndex(retrievedSet, WeapSetTrack)]
                
            ItemData["WeaponType"] = weaponType
            level = tableC[i+1].contents[0].split(" ")[1] 
            ItemData["Level"] = level[:-1] if level.find('\n') != -1 else level
           
            statTable = tableC[i+2].get_text(separator = "\n").split("\n")[:-1]
            ItemData.update(assignToDict(statTable))  
            
            currentDF = currentDF.append(ItemData, ignore_index=True)
    
    end = time.time()

    # print(currentDF)
    print(f'{weaponType} added in {end - start}')


    return WeapToJob, currentDF

def retrieveSecWeap(session):
    
    weaponLink = []
    page = session.get(defaulturl + secUrl)
    soup = BeautifulSoup(page.content, 'lxml')

    weapList = soup.find_all('a', class_="category-page__member-link")

    df = pandas.DataFrame()
    for x in weapList:
        weaponLink.append(x['href'])
        df = df.append(retreiveSecPage(x['href'], session) , ignore_index=True)

    
    df = df.fillna(0)
    


    return df

def retreiveSecPage(suburl, session):

    
    page = session.get(defaulturl + suburl)
    
    titleContent = BeautifulSoup(page.content, 'lxml').find_all('div', class_= "mw-parser-output")[0].find('a')
    weapListLink = titleContent['href']
    weapType = titleContent.get_text()

    if weapType == "Shield":
        return retrieveShield(weapListLink, session, weapType)

    start = time.time()
    
    weapListPage = session.get(defaulturl + weapListLink)

    PageContent = BeautifulSoup(weapListPage.content, 'lxml').find_all('div', class_= "mw-parser-output")[0]

    wikiTables = PageContent.find_all('table', class_='wikitable')
    headerContent = PageContent.find_all('p')


    for i in headerContent:
        if i.get_text().replace(" ", "").lower().find(weapType.lower()) != -1:
            headerP = i
            break
        if i.get_text().find('exclusive') != -1:
            headerP = i
            break
        continue

    headerPContent = list(headerP.contents)

    Jobs = []
    Cstart = False
    for i in headerPContent:
        if i.get_text().find('exclusive') != -1:
            Cstart = True
            continue
        if i.get_text().find('conjunction') != -1:
            Cstart = False
            break

        if Cstart == True:
            if i.name == 'a':
                JobName = removeN(i.next, ' ')
                Jobs.append(JobName)
 
    currentDF = pandas.DataFrame()

    ignoreList = ["evolving", "frozen"]

    if len(Jobs) == 0:
        return currentDF
    
    counter = min(len(Jobs), len(wikiTables))

    for c in range(0, counter):
        tableContent = wikiTables[c].find_all('td')
        JobType = Jobs[c]
        for i in range(0, len(tableContent), 3):
            ItemData = {}
            if JobType == "Jett":
                continue
            elif JobType == "DualBlade":
                ItemData["ClassType"] = JobType
                ItemData["WeaponType"] = weapType
                weaponSet = tableContent[i].contents
                for w in weaponSet:
                    if any(ele.lower() in w.get_text().replace(weapType, "").lower() for ele in EquipSetTrack) == True:
                        EquipName = w.get_text()
                        break
                    else:
                        EquipName = ""
                if EquipName == "":
                    continue
                ItemData["EquipName"] = EquipName
                
                EquipLevel = tableContent[i+1].get_text(separator = '\n').split('\n')[0].split(" ")[-1]
                ItemData["EquipLevel"] = EquipLevel

                EquipStat = tableContent[i+2].get_text(separator = "\n").split("\n")[:-1]
                ItemData.update(assignToDict(EquipStat))
            else:
                ItemData["ClassType"] = JobType
                ItemData["WeaponType"] = weapType
                EquipName = tableContent[i].find_all('a')[-1].get_text()
                if EquipName == "":
                    continue
                if any(ele in EquipName.lower() for ele in ignoreList) == True:
                    continue

                ItemData["EquipName"] = EquipName

                EquipLevel = tableContent[i+1].get_text(separator = '\n').split('\n')[0].split(" ")[-1]
                ItemData["EquipLevel"] = EquipLevel

                EquipStat = tableContent[i+2].get_text(separator = "\n").split("\n")[:-1]
                ItemData.update(assignToDict(EquipStat))
            currentDF = currentDF.append(ItemData, ignore_index=True)
        
    end = time.time()

    print(f"{weapType} added in {end -  start}.")
    
    return currentDF

def retrieveShield(weapListLink , session, weapType):

    start = time.time()
    
    currentDF =  pandas.DataFrame()

    for cls in Mclasses:
        newUrl = weapListLink + "/" + cls
        PageContent = session.get(defaulturl + newUrl)
        if PageContent.status_code != 200:
            continue
        currentContent = BeautifulSoup(PageContent.content, 'lxml').find_all('div', class_="wds-tab__content wds-is-current")[0]
        wikitable = currentContent.find_all('td')
        
        for i in range(0, len(wikitable), 3):
            ItemData = {}
            ItemData["ClassType"] = cls
            ItemData["WeaponType"] = weapType
            EquipName = wikitable[i].find_all('a')[-1].get_text()
            if EquipName == "":
                    continue
            ItemData["EquipName"] = EquipName

            EquipLevel = wikitable[i+1].get_text(separator = '\n').split('\n')[0].split(" ")[-1]
            if int(EquipLevel) < 110:
                continue
            ItemData["EquipLevel"] = EquipLevel

            EquipStat = wikitable[i+2].get_text(separator = "\n").split("\n")[:-1]
            ItemData.update(assignToDict(EquipStat))
            currentDF = currentDF.append(ItemData, ignore_index=True)
    end = time.time()
    print(f"Time taken to add Shield is {end - start}")
    return currentDF

def retrieveTyrant(session):
    start = time.time()

    LinksPage = session.get(defaulturl + superiorEquipUrl)

    LinksList = BeautifulSoup(LinksPage.content, 'lxml').find_all('ul', class_='category-page__members-for-char')[2].find_all('a', class_='category-page__member-link')
    tyrantAcc = pandas.DataFrame()
    tyrantArmor =  pandas.DataFrame()
    for link in LinksList:
        
        PageContent = session.get(defaulturl + link['href'])

        soup = BeautifulSoup(PageContent.content,'lxml').find_all('div', class_='mw-parser-output')[0]
        big = soup.find_all('big')[0].get_text().split(' ')
        title = big[:-1] if big[-1] == '' else big 
        equipSet = title[0]
        equipSlot = title[-1]
        if equipSlot.lower().find('boots') != -1:
            equipSlot = 'Shoes'
        elif equipSlot.lower().find('cloak') != -1:
            equipSlot = 'Cape'

        trContent = soup.find_all('tr')[2:]
        EquipData = {}
        EquipData['EquipSet'] = equipSet
        EquipData['EquipSlot'] = equipSlot
        statList = []
        for row in trContent:
            if row.find('th') and row.find('td'):
                th = removeN(row.find('th').next, '\n')
                td = removeN(row.find('td').next, '\n')
                if th.lower().find('level') != -1:
                    EquipData['EquipLevel'] = td
                    continue
                elif th.lower().find('job') != -1:
                    EquipData['ClassType'] = td
                if th.lower().find('upgrades') != -1:
                    EquipData.update(assignToDict(statList))
                    break
                statList.append(th + ': ' + td)
         
        if EquipData['EquipSlot'] in ArmorSet:
            tyrantArmor = tyrantArmor.append(EquipData, ignore_index=True)
        else:
            EquipData['EquipName'] = equipSet
            tyrantAcc = tyrantAcc.append(EquipData, ignore_index=True)

    end = time.time()
    print(f'Tyrant added in {end - start}')

    return tyrantAcc, tyrantArmor

def tryantPage(link, session):
    PageContent = session.get(defaulturl + link)

    soup = BeautifulSoup(PageContent.content,'lxml').find_all('div', class_='mw-parser-output')[0]
    big = soup.find_all('big')[0].get_text().split(' ')
    title = big[:-1] if big[-1] == '' else big 
    print(type(title))
    equipSet = title[0]
    equipSlot = title[-1]
    
    trContent = soup.find_all('tr')[2:]
    EquipData = {}
    EquipData['EquipSet'] = equipSet
    EquipData['EquipSlot'] = equipSlot
    statList = []
    for row in trContent:
        if row.find('th') and row.find('td'):
            th = removeN(row.find('th').next, '\n')
            td = removeN(row.find('td').next, '\n')
            if th.lower().find('level') != -1:
                EquipData['EquipLevel'] = td
                continue
            elif th.lower().find('job') != -1:
                EquipData['ClassType'] = td
            if th.lower().find('upgrades') != -1:
                EquipData.update(assignToDict(statList))
                break
            statList.append(th + ': ' + td)

    

    return pandas.DataFrame(EquipData)

def returnIndex(ele, setToTrack):
    for i in range(0, len(setToTrack)):
        if setToTrack[i].lower() in ele.lower():
            return i
        elif ele.lower() in setToTrack[i].lower():
            return i

def removeN(item, para):
    
    if isinstance(para, str):
        if item.find(para) != -1:
            return item.replace(para, '')
        else:
            return item
    elif isinstance(para, list):
        for i in para:
            if item.find(i) != -1:
                item = item.replace(i, '')
        return item

def assignToDict(tempList):
    
    tempData = {}
    if tempList == None:
        return tempData
    
    for i in tempList:
        if i.find("Attack Speed") != -1:
            tempData["AtkSpd"] = i.split(" ")[-1][1:-1]
        
        elif any(i.find(MS) != -1 for MS in MainStat) == True:
            if i.find(':') != -1:
                tempStr = i.split(" ")[1][1:]
            else:
                tempStr = i.split(" ")[-1][1:]
            
            if "MainStat" in tempData:
                if 'SecStat' in tempData:
                    continue
                tempData["SecStat"] = tempStr
            else:
                tempData["MainStat"] = tempStr
     
        elif i.find("HP") != -1:
            HPstr = i.split(" ")[-1][1:]
            HPstr = removeN(HPstr, ',')
            if HPstr.find("%") != -1:
                tempData['HP'] = HPstr
            else:
                tempData['HP'] = HPstr[1:]
        elif i.find("MP") != -1:
            MPstr = i.split(" ")[-1][1:]
            MPstr = removeN(MPstr, ',')
            if MPstr.find("%") != -1:
                tempData['MP'] = MPstr
            else:
                tempData['MP'] = MPstr[1:]
        elif i.find("Weapon Attack") != -1:
            tempData["ATK"] = i.split(" ")[-1][1:]

        elif i.find("Magic Attack") != -1:
            tempData["MATK"] = i.split(" ")[-1][1:]
        
        elif i.find("All Stats") != -1:
            tempData["AllStat"] = i.split(" ")[-1][1:]

        elif i.find("Boss") != -1:
            tempData["BDMG"] = i.split(" ")[-1][1:-1]

        elif i.find("Ignore") != -1:
            tempData["IED"] = i.split(" ")[-1][1:-1]

        elif i.find("Defense") != -1:
            tempData["DEF"] = i.split(" ")[-1][1:]

        elif i.find("Speed") != -1:
            tempData["SPD"] = i.split(" ")[-1][1:]

        elif i.find("Jump") != -1:
            tempData["JUMP"] = i.split(" ")[-1][1:]
        
        elif i.find("Normal") != -1:
            tempData['NDMG'] = i.split(" ")[-1][1:-1]

        elif i.find("Critical Damage") != -1:
            tempData['CDMG'] = i.split(" ")[-1][1:-1]  

    
    
    return tempData

def cleanWeapDF(WeapDF):

    WeapDF.drop_duplicates(keep='first', inplace=True)
    WeapDF = WeapDF[WeapCol]
    WeapDF = WeapDF.fillna(0)

    WeapDF.loc[(WeapDF.WeaponType == 'Bladecaster'), 'WeaponType'] = 'Tuner'
    WeapDF.loc[(WeapDF.WeaponType == 'Lucent Gauntlet'), 'WeaponType'] = 'Magic Gauntlet'
    WeapDF.loc[(WeapDF.WeaponType == 'Psy-limiter'), 'WeaponType'] = 'Psy Limiter'
    WeapDF.loc[(WeapDF.WeaponType == 'Whispershot'), 'WeaponType'] = 'Breath Shooter'
    WeapDF.loc[(WeapDF.WeaponType == 'Arm Cannon'), 'WeaponType'] = 'Revolver Gauntlet'
    WeapDF.loc[(WeapDF.WeaponType == 'Whip Blade'), 'WeaponType'] = 'Energy Sword'
    WeapDF.loc[(WeapDF.WeaponType == 'Axe'), 'WeaponType'] = 'One-Handed Axe'
    WeapDF.loc[(WeapDF.WeaponType == 'Saber'), 'WeaponType'] = 'One-Handed Sword'
    WeapDF.loc[(WeapDF.WeaponType == 'Hammer'), 'WeaponType'] = 'One-Handed Blunt Weapon'
    WeapDF.loc[(WeapDF.WeaponType == 'One-Handed Mace'), 'WeaponType'] = 'One-Handed Blunt Weapon'
    WeapDF.loc[(WeapDF.WeaponType == 'Two-Handed Mace'), 'WeaponType'] = 'Two-Handed Blunt Weapon'
    WeapDF.loc[(WeapDF.WeaponType == 'Two-Handed Hammer'), 'WeaponType'] = 'Two-Handed Blunt Weapon'


    return WeapDF

def cleanSetEffectDF(SEDF):

    SEDF.drop_duplicates(keep='first', inplace=True)
    SEDF = SEDF[SetCol]
    SEDF = SEDF.fillna(0)

    return SEDF

def cleanWTJDF(DF):

    DF.drop_duplicates(keep='first', inplace=True)

    DF.loc[(DF.WeaponType == 'Bladecaster'), 'WeaponType'] = 'Tuner'
    DF.loc[(DF.WeaponType == 'Lucent Gauntlet'), 'WeaponType'] = 'Magic Gauntlet'
    DF.loc[(DF.WeaponType == 'Psy-limiter'), 'WeaponType'] = 'Psy Limiter'
    DF.loc[(DF.WeaponType == 'Whispershot'), 'WeaponType'] = 'Breath Shooter'
    DF.loc[(DF.WeaponType == 'Arm Cannon'), 'WeaponType'] = 'Revolver Gauntlet'
    DF.loc[(DF.WeaponType == 'Whip Blade'), 'WeaponType'] = 'Energy Sword'
    DF.loc[(DF.WeaponType == 'Axe'), 'WeaponType'] = 'One-Handed Axe'
    DF.loc[(DF.WeaponType == 'Saber'), 'WeaponType'] = 'One-Handed Sword'
    DF.loc[(DF.WeaponType == 'Hammer'), 'WeaponType'] = 'One-Handed Blunt Weapon'
    DF.loc[(DF.WeaponType == 'One-Handed Mace'), 'WeaponType'] = 'One-Handed Blunt Weapon'
    DF.loc[(DF.WeaponType == 'Two-Handed Mace'), 'WeaponType'] = 'Two-Handed Blunt Weapon'
    DF.loc[(DF.WeaponType == 'Two-Handed Hammer'), 'WeaponType'] = 'Two-Handed Blunt Weapon'

    DF.loc[(DF.JobType == 'Arch Mage (Fire, Poison)'), 'JobType'] = 'Fire Poison'
    DF.loc[(DF.JobType == 'Arch Mage (Ice, Lightning)'), 'JobType'] = 'Ice Lightning'


    DF.drop(DF.loc[DF['JobType']=='Jett'].index, inplace = True)
    return DF

main()
# retrieveTyrant(requests.session())