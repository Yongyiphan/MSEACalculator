# from numpy import fabs
# import pandas
# import requests
# import lxml
# import cchardet
# import time
# from bs4 import BeautifulSoup

# ### PROGRAM FLOW ####
# # MAIN PAGE -> GET EQUIPNAME/LINK 
# # TRAVERSE TO EQUIPLINK PAGE -> GET WEAPON SET'S LINK
# # TRAVERSE TO WEAPON'LINK -> RECORD INFORMATION
# defaulturl = "https://maplestory.fandom.com"
# weaponUrl = "/wiki/Category:Weapons"
# secUrl = "/wiki/Category:Secondary_Weapons"
# equipSetUrl = "/wiki/Category:Equipment_Sets"
# setToTrack = ['Genesis', 'Arcane', 'Utgard','Absolab', 'Fafnir', 'Lapis','Lazuli', '']
# Mclasses = ['Warrior', 'Bowman', 'Magician','Thief', 'Pirate']
# trackMinLevel = 140

# def main():
    
#     start = time.time()
    
#     # weapDF = retrieveMainWeap()
#     # weapDF.to_csv("WeaponData.csv", encoding='utf-8')
#     secDf = retrieveSecWeap()
#     secDf.to_csv("SecondaryWeapData.csv", encoding = 'utf-8')
#     end = time.time()

#     print(f"Time take: {end - start}")
#     return

# def retrieveMainWeap():

#     request_session = requests.sessions.session()
    
#     page = request_session.get(defaulturl + weaponUrl)

#     soup  = BeautifulSoup(page.content, "lxml")
#     weaponList = list(soup.find_all('a', class_='category-page__member-link'))
    
#     weaponLink = []

#     df = pandas.DataFrame()
#     for x in weaponList:
#         if(x['href'].find("Secondary_Weapons") != -1):
#             continue
#         weaponLink.append(x['href'])
#         df = df.append(retreiveWeapPage(x['href'], request_session, setToTrack), ignore_index=True)
    
    
#     df = df.fillna(0)
    
#     df.loc[(df.WeaponType == 'Bladecaster'), 'WeaponType'] = 'Tuner'
#     df.loc[(df.WeaponType == 'LucentGauntlet'), 'WeaponType'] = 'MagicGauntlet'
#     df.loc[(df.WeaponType == 'Psy-limiter'), 'WeaponType'] = 'PsyLimiter'
#     df.loc[(df.WeaponType == 'Whispershot'), 'WeaponType'] = 'BreathShooter'


#     return df

# def retreiveWeapPage(suburl, session, trackSet):

#     start = time.time()
#     page = session.get(defaulturl + suburl)
    
#     PageContent = BeautifulSoup(page.content, 'lxml').find_all('div', class_= "mw-parser-output")
#     weapListURL = PageContent[0].find_all('a')[0]['href']
#     weapType = PageContent[0].find_all('a')[0].get_text()
#     weapListPage = session.get(defaulturl + weapListURL)
#     soup = BeautifulSoup(weapListPage.content, "lxml")
#     tableC =  soup.find_all('table', class_="wikitable")[0].find_all('td')   

#     headerP = soup.find_all('div', class_="mw-parser-output")[0].find_all('p')[0]
#     headerLinks = headerP.find_all('a')
    
#     JobType = ""
#     headerPcontent = list(headerP.contents)
    
#     if headerP.get_text().lower().find("exclusive") != -1:
#         for i in range(0, len(headerPcontent)):
#             if  headerPcontent[i].get_text().find('exclusive') != -1:
#                 JobType = headerPcontent[i+1].contents[0].replace(" ", "") if headerPcontent[i+1].contents[0].find(" ") != -1 else headerPcontent[i+1].contents[0]
#     elif headerP.get_text().lower().find("conjunction") != -1:
#         JobType = headerLinks[0].get_text()
#     elif headerP.get_text().lower().find("bow") != -1:
#         JobType = "Bowman"
#     elif headerP.get_text().lower().find("dagger") != -1:
#         JobType = "Thief"
#     elif headerP.get_text().lower().find("knuckles") != -1:
#         JobType = "Pirate"
#     elif headerP.get_text().lower().find("claw") != -1:
#         JobType = "Thief"
#     else:
#         for ele in Mclasses:
#             if headerP.get_text().lower().find(ele.lower()) != -1:
#                 JobType = ele
                
#     currentDF = pandas.DataFrame()
    
#     for i in range(0, len(tableC),3):
#         ItemData = {}
#         ItemData["ClassType"] = JobType
#         if any(ele.lower() in tableC[i].get_text().lower() for ele in setToTrack) == True:
#             retrievedSet = tableC[i].get_text()[:-1].replace(weapType, "")
#             if retrievedSet.lower().find("sealed") != -1:
#                 continue     
#             if retrievedSet.lower().find("utgard") != -1:
#                 ItemData["WeaponSet"] = "Fensalir"
#             elif retrievedSet.lower().find("lapis") != -1 or retrievedSet.lower().find("lazuli") != -1:
#                 ItemData["WeaponSet"] = "".join(retrievedSet.split(" ")[1:])
#             else:
#                 ItemData["WeaponSet"] = setToTrack[returnIndex(retrievedSet)]
#             ItemData["WeaponType"] = weapType.replace(" ","") if weapType.find(" ") else weapType
#             level = tableC[i+1].contents[0].split(" ")[1] 
#             ItemData["Level"] = level[:-1] if level.find('\n') != -1 else level
#             if (int(ItemData["Level"]) < trackMinLevel) and ItemData["ClassType"].lower() != "zero":
#                 continue
#             statTable = tableC[i+2].get_text(separator = "\n").split("\n")[:-1]
#             ItemData.update(assignToDict(statTable))  
#             if not currentDF.empty:
#                 if  ItemData["ClassType"] in currentDF.values and ItemData["WeaponSet"] in currentDF.values:
#                     continue
#                 else:
#                     currentDF = currentDF.append(ItemData, ignore_index=True)
#             else:              
#                 currentDF = currentDF.append(ItemData, ignore_index=True)
    
#     end = time.time()
#     print(f"Time taken for {weapType} to be added is  {end - start}")
      
#     return currentDF
    
#     # return

# def retrieveSecWeap():

#     weaponLink = []

#     request_session = requests.session()
#     page = request_session.get(defaulturl + secUrl)
#     soup = BeautifulSoup(page.content, 'lxml')

#     weapList = soup.find_all('a', class_="category-page__member-link")

#     df = pandas.DataFrame()
#     for x in weapList:
#         weaponLink.append(x['href'])
#         df = df.append(retreiveSecPage(x['href'], request_session) , ignore_index=True)

    
#     df = df.fillna(0)
    


#     return df

# def retreiveSecPage(suburl, session):

#     start = time.time()
#     page = session.get(defaulturl + suburl)
    
#     titleContent = BeautifulSoup(page.content, 'lxml').find_all('div', class_= "mw-parser-output")[0].find('a')
#     weapListLink = titleContent['href']
#     weapType = titleContent.get_text().replace(" ", "")

#     if weapType == "Shield":
#         return retrieveShield(weapListLink, session, weapType)

#     weapListPage = session.get(defaulturl + weapListLink)

#     PageContent = BeautifulSoup(weapListPage.content, 'lxml').find_all('div', class_= "mw-parser-output")[0]

#     wikiTables = PageContent.find_all('table', class_='wikitable')
#     headerContent = PageContent.find_all('p')


#     for i in headerContent:
#         if i.get_text().replace(" ", "").lower().find(weapType.lower()) != -1:
#             headerP = i
#             break
#         if i.get_text().find('exclusive') != -1:
#             headerP = i
#             break
#         continue


#     headerPContent = list(headerP.contents)

#     Jobs = []
#     Cstart = False
#     for i in headerPContent:
#         if i.get_text().find('exclusive') != -1:
#             Cstart = True
#             continue
#         if i.get_text().find('conjunction') != -1:
#             Cstart = False
#             break

#         if Cstart == True:
#             if i.name == 'a':
#                 JobName = i.next.replace(" ", "") if i.next.find(" ") != -1 else i.next
#                 Jobs.append(JobName)
 
#     currentDF = pandas.DataFrame()

#     ignoreList = ["evolving", "frozen"]

#     if len(Jobs) == 0:
#         return currentDF
    
#     counter = min(len(Jobs), len(wikiTables))

#     for c in range(0, counter):
#         tableContent = wikiTables[c].find_all('td')
#         JobType = Jobs[c]
#         for i in range(0, len(tableContent), 3):
#             ItemData = {}
#             if JobType == "Jett":
#                 continue
#             elif JobType == "DualBlade":
#                 ItemData["ClassType"] = JobType
#                 ItemData["WeaponType"] = weapType
#                 weaponSet = tableContent[i].contents
#                 for w in weaponSet:
#                     if any(ele.lower() in w.get_text().replace(weapType, "").lower() for ele in setToTrack) == True:
#                         EquipName = w.get_text()
#                         break
#                     else:
#                         EquipName = ""
#                 if EquipName == "":
#                     continue
#                 ItemData["EquipName"] = EquipName
                
#                 EquipLevel = tableContent[i+1].get_text(separator = '\n').split('\n')[0].split(" ")[-1]
#                 ItemData["EquipLevel"] = EquipLevel

#                 EquipStat = tableContent[i+2].get_text(separator = "\n").split("\n")[:-1]
#                 ItemData.update(assignToDict(EquipStat))
#             else:
#                 ItemData["ClassType"] = JobType
#                 ItemData["WeaponType"] = weapType
#                 EquipName = tableContent[i].find_all('a')[-1].get_text()
#                 if EquipName == "":
#                     continue
#                 if any(ele in EquipName.lower() for ele in ignoreList) == True:
#                     continue

#                 ItemData["EquipName"] = EquipName

#                 EquipLevel = tableContent[i+1].get_text(separator = '\n').split('\n')[0].split(" ")[-1]
#                 ItemData["EquipLevel"] = EquipLevel

#                 EquipStat = tableContent[i+2].get_text(separator = "\n").split("\n")[:-1]
#                 ItemData.update(assignToDict(EquipStat))
#             currentDF = currentDF.append(ItemData, ignore_index=True)

#     end = time.time()
    
#     print(f"Time taken for {weapType} to be added is  {end - start}")  
#     return currentDF

# def retrieveShield(weapListLink , session, weapType):

#     currentDF =  pandas.DataFrame()

#     for cls in Mclasses:
#         newUrl = weapListLink + "/" + cls
#         PageContent = session.get(defaulturl + newUrl)
#         if PageContent.status_code != 200:
#             continue
        
#         currentContent = BeautifulSoup(PageContent.content, 'lxml').find_all('div', class_="wds-tab__content wds-is-current")[0]

#         wikitable = currentContent.find_all('td')
        
#         for i in range(0, len(wikitable), 3):
#             ItemData = {}
#             ItemData["ClassType"] = cls
#             ItemData["WeaponType"] = weapType
#             EquipName = wikitable[i].find_all('a')[-1].get_text()
#             if EquipName == "":
#                     continue
#             ItemData["EquipName"] = EquipName

#             EquipLevel = wikitable[i+1].get_text(separator = '\n').split('\n')[0].split(" ")[-1]
#             if int(EquipLevel) < 110:
#                 continue
#             ItemData["EquipLevel"] = EquipLevel

#             EquipStat = wikitable[i+2].get_text(separator = "\n").split("\n")[:-1]
#             ItemData.update(assignToDict(EquipStat))
#             currentDF = currentDF.append(ItemData, ignore_index=True)
#     return currentDF

# def retrieveGenesis(session):
    
#     page = session.get(defaulturl + genesisWeapUrl)
#     soup = BeautifulSoup(page.content, 'lxml')
    
#     mainWeapLinksA = soup.find_all('tbody')[0].find_all('td')[4].find_all('div')[0].find_all('li')
    
#     currentDF = pandas.DataFrame()
#     weapToUser = {}
#     weaponLinkList = {}
#     JobType = []
#     WeaponType = []
#     for i in mainWeapLinksA:
#         links = i.find_all('a')[1:] #remove image
#         if removeN(links[0].next, 'Sealed Genesis').lower().find('scepter') != -1:
#             continue
#         tempL = []
#         for j in range(1, len(links)):
#             tempL.append(links[j].next)
            
#             JobType.append(links[j].next)
#             WT = removeN(links[0].next, 'Sealed Genesis')
#             if WT.split(' ')[0] == '' or WT.split(' ')[0] == ' ':
#                 WT = WT[1:]
#             WeaponType.append(WT)

#         currentDF = currentDF.append(retrieveGWeap(links[0]['href'], session, tempL))

#     weapToUser['JobType'] = JobType
#     weapToUser['WeaponType'] = WeaponType
#     weapToUserDF = pandas.DataFrame(weapToUser)
#     return weapToUserDF, currentDF

# def retrieveGWeap(subUrl, session, weapUser):

#     start = time.time()
#     currentDF =  pandas.DataFrame()
#     PageContent = session.get(defaulturl + subUrl)
    
#     soup = BeautifulSoup(PageContent.content, 'lxml')

#     title = soup.find_all('h1', class_='page-header__title')[0].get_text()
#     weaponSet = 'Genesis'
#     weaponType = removeN(title, ['\n','\t',weaponSet])

#     if weaponType.split(' ')[0] == '' or weaponType.split(' ')[0] == ' ':
#         weaponType = weaponType[1:]

#     tbody = soup.find_all('div', class_='mw-parser-output')[0]
#     tableContent = tbody.find_all('table')
    
#     if 'Xenon' in weapUser:
#         header = tbody.find_all('h3')
#         wikitable = [tableContent[0], tableContent[1]]

#         for i in range(0,2):
#             weapData = {}
#             statList = []
#             trContent = wikitable[i].find_all('tr')
#             for j in trContent:
#                 if j.find('th') and j.find('td'):
#                     # pairV = removeN(i.get_text(separator ='\n'), '\n')
#                     if j.find('td').find('a'):
#                         continue
#                     th = removeN(j.find('th').next, '\n')
#                     if any(th.find(MS) != -1 for MS in MainStat) == True and th.find('REQ') != -1:
#                         continue
#                     td = removeN(j.find('td').next,'\n')
#                     statList.append(' '.join([th, td]))
#             weapData['WeaponSet'] = weaponSet
#             weaponType = 'Energy Sword'
#             weapData['WeaponType'] = weaponType + '(' + header[i].get_text().split(' ')[0] + ')'
#             weapData.update(assignToDict(statList))
#             currentDF = currentDF.append(weapData, ignore_index=True)

#     else:
#         wikitable = tableContent[0]
#         trContent = wikitable.find_all('tr')
#         weapData = {}  
#         statList = []
#         for i in trContent:
#             if i.find('th') and i.find('td'):
#                 # pairV = removeN(i.get_text(separator ='\n'), '\n')
#                 if i.find('td').find('a'):
#                     continue
#                 th = removeN(i.find('th').next, '\n')
#                 if any(th.find(MS) != -1 for MS in MainStat) == True and th.find('REQ') != -1:
#                     continue
#                 td = removeN(i.find('td').next,'\n')

#                 if th.find('Level') != -1:
#                     weapData['Level'] = td
#                     continue
#                 statList.append(' '.join([th, td]))
#         weapData['WeaponSet'] = weaponSet
#         if weaponType == 'Guards':
#             weaponType = 'Claw'
#         elif weaponType == 'Claw':
#             weaponType = 'Knuckle'
#         elif weaponType == 'Ellaha':
#             weaponType = 'Arm Cannon'
#         elif weaponType == 'Siege Gun':
#             weaponType = 'Hand Cannon'
#         elif weaponType == 'Pistol':
#             weaponType = 'Gun'
        

#         weapData['WeaponType'] = weaponType
#         weapData.update(assignToDict(statList))
#         currentDF = currentDF.append(weapData, ignore_index=True)

    

#     end = time.time()
#     print(f'Genesis {weaponType} added in {end - start}.')
#     return currentDF

# def removeN(item, para):
    
#     if isinstance(para, str):
#         if item.find(para) != -1:
#             return item.replace(para, '')
#         else:
#             return item
#     elif isinstance(para, list):
#         for i in para:
#             if item.find(i) != -1:
#                 item = item.replace(i, '')
#         return item


# def returnIndex(ele):
#     for i in range(0, len(setToTrack)):
#         if setToTrack[i].lower() in ele.lower():
#             return i
#         elif ele.lower() in setToTrack[i].lower():
#             return i

# def assignToDict(tempList):
    
#     tempData = {}
#     if tempList == None:
#         return tempData
    
#     for i in tempList:
#         if i.find("Attack Speed") != -1:
#             tempData["AtkSpd"] = i.split(" ")[-1][1:-1]
#             continue
#         elif i.find("STR") != -1:
#             if "MainStat" in tempData:
#                 tempData["SecStat"] = i.split(" ")[1][1:]
#             else:
#                 tempData["MainStat"] = i.split(" ")[1][1:]
#             continue
#         elif i.find("DEX") != -1:
#             if "MainStat" in tempData:
#                 tempData["SecStat"] = i.split(" ")[1][1:]
#             else:
#                 tempData["MainStat"] = i.split(" ")[1][1:]
#             continue       
#         elif i.find("INT") != -1:
#             if "MainStat" in tempData:
#                 tempData["SecStat"] = i.split(" ")[1][1:]
#             else:
#                 tempData["MainStat"] = i.split(" ")[1][1:]
#             continue
#         elif i.find("LUK") != -1:
#             if "MainStat" in tempData:
#                 tempData["SecStat"] = i.split(" ")[1][1:]
#             else:
#                 tempData["MainStat"] = i.split(" ")[1][1:]
#             continue
#         elif i.find("HP") != -1:
#             tempData["HP"] = i.split(" ")[-1][1:].replace(",", "")
#             continue
#         elif i.find("Weapon Attack") != -1:
#             tempData["Atk"] = i.split(" ")[-1][1:]
#             continue
#         elif i.find("Magic Attack") != -1:
#             tempData["MAtk"] = i.split(" ")[-1][1:]
#             continue
#         elif i.find("Boss") != -1:
#             tempData["BossDMG"] = i.split(" ")[-1][1:-1]
#             continue
#         elif i.find("Ignore") != -1:
#             tempData["IED"] = i.split(" ")[-1][1:-1]
#             continue
#         elif i.find("Speed") != -1:
#             tempData["SPD"] = i.split(" ")[-1][1:]
#             continue
#         elif i.find("Defense") != -1:
#             tempData["DEF"] = i.split(" ")[-1][1:]
#             continue
#         elif i.find("All Stats") != -1:
#             tempData["AS"] = i.split(" ")[-1][1:]
#             continue
        
    
    
#     return tempData


# main()
