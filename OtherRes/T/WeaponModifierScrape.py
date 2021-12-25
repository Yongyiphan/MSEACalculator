import pandas
import lxml
import cchardet
import time
from bs4 import BeautifulSoup
import requests
from requests.api import request


def main():
    
    url = "https://strategywiki.org/wiki/MapleStory/Formulas"

    start = time.time()
    page = requests.get(url)
    soup = BeautifulSoup(page.content, 'lxml')
    weaponName = []
    weaponMod = []
    
    tables = soup.find_all('table', class_='wikitable prettytable')[0]
    itemName = tables.find_all('th')
    itemValue = tables.find_all('td')
    
    for i in range(0, len(itemName),1):
        if(itemName[i].get_text().split(",'")):
            splitList = itemName[i].get_text().split(",")
            for x in splitList:
                if x.find("\n") != -1:
                    weaponName.append(x[:-1])
                else:
                    weaponName.append(x)
                weaponMod.append(itemValue[i].get_text()[:-1])
        else:
            weaponName.append(itemName[i].get_text())
            weaponMod.append(itemValue[i].get_text()[:-1])
            

    #Clean up 
    for i in range(0,len(weaponName)):
        if weaponName[i].find("(") != -1:
            weaponName[i] =" ".join(weaponName[i].split(" ")[:-1])
    
    for i in range(0, len(weaponName)):
        if weaponName[i].find("/") != -1:
            weaponName[i] = weaponName[i].split("/")[0]
            if weaponName[i].find(" ") != -1:
                weaponName[i] = weaponName[i].replace(" ", "")
        elif weaponName[i].find("_") != -1:
            weaponName[i] = weaponName[i].replace("_", "")
        elif weaponName[i].find(" ") != -1:
            weaponName[i] = weaponName[i].replace(" ", "")
        


    for i in range(0, len(weaponMod)):
        if weaponMod[i].find("(") != -1:
            weaponMod[i] = "".join(weaponMod[i].split(" ")[0])
    # print(weaponName)
    
    data ={
        "Weapon Name" : weaponName,
        "Modifier" : weaponMod
    }
    
    df = pandas.DataFrame(data)
    
    print(df)

    df.to_csv('WeapMod.csv', encoding='utf-8')
    
    end = time.time()

    print(f"Time taken: {end - start} ")
    
    return


main()