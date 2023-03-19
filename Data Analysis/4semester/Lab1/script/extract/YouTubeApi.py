from ConfigProgram import ConfigProgram
from googleapiclient.discovery import build
import pandas as pd

class YouTubeApi:

    def __init__(self, api_key:str) -> None:
        self.resource=build('youtube', 'v3', developerKey=api_key)

    def get_comments(self, video_id:str, count:int, comment_attr:list) :
        comments_df = pd.DataFrame(columns=[col_name.split('.')[-1] for col_name in comment_attr])
        next_page_token = None
        root_attr_name=comment_attr[0].split('.')[0]


        while count>0:
            results = self.resource.commentThreads().list(
                part=[part for i, part in enumerate(ConfigProgram.available_comment_part_values) if i in ConfigProgram.comment_part_indexes],
                videoId=video_id,
                maxResults=count,
                pageToken=next_page_token
            ).execute()
            rows = []
            for item in results[root_attr_name]:
                row = {}
                for col_name in comment_attr:
                    split_attr_names=col_name.split('.')
                    eval_str=None
                    if not split_attr_names[1:-1]:
                        eval_str='item.get("'+split_attr_names[-1]+'")'
                    else:
                        eval_str='item["' + '"]["'.join(split_attr_names[1:-1]) + '"].get("'+split_attr_names[-1]+'")'
                    value = eval(eval_str)
                    row[split_attr_names[-1]] = value
                    # value = eval('item["' + '"]["'.join(col_name.split('.')[1:]) + '"]')
                    # row[col_name.split('.')[-1]] = value
                
                rows.append(row)

            comments_df = pd.concat([comments_df, pd.DataFrame(rows)])
            count= count-len(rows)
            if 'nextPageToken' in results :
                next_page_token = results['nextPageToken']
            else:
                count=0

        return comments_df
    
    def get_videos(self, channel_id:str, count:int, video_attr:list[str]) -> pd.DataFrame:
        videos_df = pd.DataFrame(columns=[col_name.split('.')[-1] for col_name in video_attr])

        search_results = self.resource.search().list(
                part='id',
                channelId=channel_id,
                type='video',
                order='viewCount',
                maxResults=count
            ).execute()
        video_ids = [video['id']['videoId'] for video in search_results['items']]
        video_stats = self.resource.videos().list(
                            part= [part for i, part in enumerate(ConfigProgram.available_video_part_values) if i in ConfigProgram.video_part_indexes],
                            id=','.join(video_ids),
                            maxResults=count
                        ).execute()

        root_attr_name = video_attr[0].split('.')[0]
        rows = []
        for item in video_stats[root_attr_name]:
            row = {}
            for col_name in video_attr:
                split_attr_names=col_name.split('.')
                eval_str=None
                if not split_attr_names[1:-1]:
                    eval_str='item.get("'+split_attr_names[-1]+'")'
                else:
                    eval_str='item["' + '"]["'.join(split_attr_names[1:-1]) + '"].get("'+split_attr_names[-1]+'")'
                value = eval(eval_str)
                row[split_attr_names[-1]] = value
            
            rows.append(row)
        
        videos_df = pd.concat([videos_df, pd.DataFrame(rows)])           
        
        return videos_df