from dataclasses import dataclass
from typing import List

class ConfigProgram:
    num_videos: int=10  
    num_comments: int=100

    #part parameter available values
    available_comment_part_values: list[str]=['id', 'replies', 'snippet']
    available_video_part_values: list[str] = ['contentDetails', 'id', 'liveStreamingDetails', 'recordingDetails', 'snippet', 'statistics', 'status', 'topicDetails']

    comment_attr_indexes=[12,13,21,25]
    video_attr_indexes=[] #TODO: add indexes

    comment_part_indexes=[2]
    video_part_indexes=[0,1,4,5]
     
