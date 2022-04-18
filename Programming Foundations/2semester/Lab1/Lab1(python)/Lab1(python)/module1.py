#запис у файл тексту з перевіркою на комбінацію клавіш
def ReadMultipleInp(str,file):
    i=0
    while i<len(str):
        if str[i]==chr(5):
            str=str[:i:]       
            if(len(str)!=0):
                file.write(str+'\n')
            return 0
        i+=1
    file.write(str+'\n')
    return 1

def appendFile(name):
    print("Press ctrl+E to stop the edit. \nPlace your input here:")
    cont=1
    file = open(name,"a")
    while(cont):
        str_text=input()
        cont = ReadMultipleInp(str_text,file)
    file.close()

def rewriteFile(name):
    open(name, "w").close()
    appendFile(name)

#читання файлу
def printContents(name):
    print("\nread file",name[name.rfind('/')+1:],":")
    file=open(name,"r")
    for i in file.readlines():
        print(i, end=' ')

def InvertBinaryDigits(fileName, newFileName):
    streamReader = open(fileName)
    streamWriter = open(newFileName, "w")
    for i in streamReader.readlines():
        index=0
        for j in i:
            if j == '0':
                i=i[:index]+'1'+i[index+1:]
            elif j== '1':
                i=i[:index]+'0'+i[index+1:]
            index+=1
        streamWriter.write(i)
    streamReader.close()
    streamWriter.close()