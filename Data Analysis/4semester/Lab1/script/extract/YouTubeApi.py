from .ExtractConfig import ExtractConfig
from googleapiclient.discovery import build
import pandas as pd


class YouTubeApi:

    def __init__(self, api_key: str) -> None:
        self.resource = build('youtube', 'v3', developerKey=api_key)

    # cost=1
    def get_comments(self, video_id: str, count: int, comment_attr: list):
        comments_df = pd.DataFrame(columns=[col_name.split('.')[-1] for col_name in comment_attr])
        next_page_token = None
        root_attr_name = comment_attr[0].split('.')[0]
        if count > 100:
            raise ValueError(
                "The limitation for amount of comments is set to 100 by the package creator. You can change the code of get_comments to process paged responses if you need to")

        results = self.resource.commentThreads().list(
            part=[part for i, part in enumerate(ExtractConfig.available_comment_part_values) if
                  i in ExtractConfig.comment_part_indexes],
            videoId=video_id,
            maxResults=count,
            pageToken=next_page_token
        ).execute()
        rows = []
        for item in results[root_attr_name]:
            try:
                row = {}
                for col_name in comment_attr:
                    split_attr_names = col_name.split('.')
                    eval_str = None
                    if not split_attr_names[1:-1]:
                        eval_str = 'item.get("' + split_attr_names[-1] + '")'
                    else:
                        eval_str = 'item["' + '"]["'.join(split_attr_names[1:-1]) + '"].get("' + split_attr_names[
                            -1] + '")'
                    value = eval(eval_str)
                    row[split_attr_names[-1]] = value
                    # value = eval('item["' + '"]["'.join(col_name.split('.')[1:]) + '"]')
                    # row[col_name.split('.')[-1]] = value

                rows.append(row)
            except:
                continue

        comments_df = pd.concat([comments_df, pd.DataFrame(rows)])

        return comments_df


# cost=1*len(video_ids)/50
def get_videos(self, video_ids, video_attr: list[str]) -> pd.DataFrame:
    MAX_COUNT_IN_BATCH = 50
    videos_df = pd.DataFrame(columns=[col_name.split('.')[-1] for col_name in video_attr])
    rows = []
    root_attr_name = video_attr[0].split('.')[0]
    batch_list = [video_ids[i:i + MAX_COUNT_IN_BATCH] for i in range(0, len(video_ids), MAX_COUNT_IN_BATCH)]

    for batch in batch_list:
        parts_to_extract = [part for i, part in enumerate(ExtractConfig.available_video_part_values) if
                            i in ExtractConfig.video_part_indexes]
        batch = ','.join(batch)
        video_stats = self.resource.videos().list(
            part=parts_to_extract,
            id=batch
        ).execute()

        for item in video_stats[root_attr_name]:
            row = {}
            for col_name in video_attr:
                split_attr_names = col_name.split('.')
                eval_str = None
                if not split_attr_names[1:-1]:
                    eval_str = 'item.get("' + split_attr_names[-1] + '")'
                else:
                    eval_str = 'item["' + '"]["'.join(split_attr_names[1:-1]) + '"].get("' + split_attr_names[
                        -1] + '")'
                value = eval(eval_str)
                row[split_attr_names[-1]] = value

            rows.append(row)

    videos_df = pd.concat([videos_df, pd.DataFrame(rows)])
    return videos_df


def get_channel_id(self, channel_name: str) -> str:
    return self.resource.search().list(
        q=channel_name,
        type='channel',
        part='id'
    ).execute()['items'][0]['id']['channelId']
