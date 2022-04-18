import datetime
import keyboard
import pickle
from ProgramTV import *
import msvcrt
import sys

def DisplayFilePrograms(programTVs: list[ProgramTV]):
    for program in programTVs:
        program.Print()

def GetFilePrograms(filePath: str):
    fileContent=list()
    with open(filePath, "rb") as reader:
        print(f"Reading content of file \"{filePath}\".")
        fileContent=pickle.load(reader)
    return fileContent
        

def WriteToFile(file: str, someList: list[ProgramTV] , mode: str="wb"):
    with open(file, mode) as writer:
        pickle.dump(someList, writer)



def GetInputPrograms():
    myProgrames=list()
    while True:
        answ=input("Do you want to add another program?(y/n)")
        if(answ=="y"):
            myProgrames.append(GetProgramTVFromConsole())
        else:
            break 
    print()
    return myProgrames


def GetProgramTVFromConsole():
    inp = ProgramTV()

    inp.title = input("Enter program name: ")
    inp.startTime = datetime.datetime.strptime(input("Enter program start time: "), "%H:%M")
    inp.endTime = datetime.datetime.strptime(input("Enter program end time: ") , "%H:%M")
    keyboard.clear_all_hotkeys
    sys.stdout.flush()
    while msvcrt.kbhit():
        msvcrt.getch()

    inp.timeSpan = inp.endTime-inp.startTime
    return inp
