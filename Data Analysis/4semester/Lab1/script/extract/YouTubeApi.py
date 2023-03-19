from ConfigProgram import ConfigProgram
from googleapiclient.discovery import build
import googleapiclient.discovery
import pandas as pd

class YouTubeApi:

    def __init__(self, api_key:str) -> None:
        self.resource=build('youtube', 'v3', developerKey=api_key)

    def get_comments(self, video_id:str, count:int, comment_attr:list) :
        comments_df = pd.DataFrame(columns=[col_name.split('.')[-1] for col_name in comment_attr])
        next_page_token = None

        while count>0:
            results = self.resource.commentThreads().list(
                part=[part for i, part in enumerate(ConfigProgram.available_comment_part_values) if i in ConfigProgram.comment_part_indexes],
                videoId=video_id,
                maxResults=count,
                pageToken=next_page_token
            ).execute()
            for item in results['items']:
                comment = item['snippet']['topLevelComment']['snippet']['textDisplay']
                like_count = item['snippet']['topLevelComment']['snippet']['likeCount']
                reply_count = item['snippet']['totalReplyCount']
                # Append row to DataFrame
                comments_df = comments_df.append({'video_id': video_id, 'comment': comment, 'like_count': like_count, 'reply_count': reply_count}, ignore_index=True)

            count= count-len(comments_df.index)
            if 'nextPageToken' in results :
                next_page_token = results['nextPageToken']
            else:
                count=0

        return comments_df