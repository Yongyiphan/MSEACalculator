import pandas
import requests
import lxml
import cchardet
import time
from bs4 import BeautifulSoup


mainUrl = 'https://grandislibrary.com'
classesUrl = 'https://grandislibrary.com/classes'




def main():





    return

def navigateClasses():
    request_session = requests.session()

    mainPage =  request_session.get(classesUrl)

    soup = BeautifulSoup(mainPage.content, 'lxml')
    ClassesLinks = soup.find_all('div')

    Linkslist = []

    for d in ClassesLinks:
        temp = d.contents[0] 


    print(ClassesLinks)
    return

# main()
navigateClasses()