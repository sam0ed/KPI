import pickle
from module import *
from ProgramTV import *


def main():
    filePath1="firstFile"
    filePath2="secondFile"

    WriteToFile(filePath1, GetInputPrograms())
    fileContent:list[ProgramTV]=GetFilePrograms(filePath1)
    DisplayFilePrograms(fileContent)

    chosenContent:list[ProgramTV]=[item for item in fileContent if item.startTime.hour>=9 and item.endTime.hour<18 ]
    WriteToFile(filePath2, chosenContent)
    DisplayFilePrograms(GetFilePrograms(filePath2))

    
if __name__=="__main__":
    main()