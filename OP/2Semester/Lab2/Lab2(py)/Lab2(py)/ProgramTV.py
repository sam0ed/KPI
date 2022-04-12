import datetime

class ProgramTV(object):
    def __init__(self):
        self.title=''
        self.startTime=datetime.datetime.now()
        self.endTime=datetime.datetime.now()
        self.timeSpan=self.endTime-self.startTime

    def Print(self):
        print(f"Title: {self.title}  Start: {self.startTime.time()}  End: {self.endTime.time()}  Lasts: {self.timeSpan}")
   



