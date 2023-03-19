import os
from typing import Literal
from googleapiclient.discovery import build
import pandas as pd
import configparser

# create extracted folder if it does not exist
path_to_extracted = os.path.abspath(os.path.join(
    os.path.dirname(__file__), '../..', 'data/extracted'))
if not os.path.exists(path_to_extracted):
    os.makedirs(path_to_extracted)

# Set up the API client
config = configparser.ConfigParser()
config.read(r'G:\My Drive\api_keys.ini')
api_key = config['Google']['api_key_1']
youtube = build('youtube', 'v3', developerKey=api_key)

# Define the parameters for the API request
channel_id = 'UC-lHJZR3Gqxm24_Vd_AJ5Yw'  # PewDiePie's channel ID
num_videos = 10  # Get top 10 viewed videos
num_comments = 100


# Define a function to retrieve the top 100 comments and their information for a given video ID
def get_comments(video_id, count: int):
    comments_df = pd.DataFrame(
        columns=['video_id', 'comment', 'like_count', 'reply_count'])
    next_page_token = None

    while count > 0:
        results = youtube.commentThreads().list(
            part='snippet',
            videoId=video_id,
            maxResults=count,
            pageToken=next_page_token
        ).execute()
        for item in results['items']:
            comment = item['snippet']['topLevelComment']['snippet']['textDisplay']
            like_count = item['snippet']['topLevelComment']['snippet']['likeCount']
            reply_count = item['snippet']['totalReplyCount']
            # Append row to DataFrame
            comments_df = comments_df.append(
                {'video_id': video_id, 'comment': comment, 'like_count': like_count, 'reply_count': reply_count}, ignore_index=True)

        count = count-len(comments_df.index)
        if 'nextPageToken' in results:
            next_page_token = results['nextPageToken']
        else:
            count = 0

    return comments_df


videos = youtube.search().list(
    part='id',
    channelId=channel_id,
    type='video',
    order='viewCount',
    maxResults=num_videos
).execute()

# Extract the video IDs from the API response
video_ids = [video['id']['videoId'] for video in videos['items']]

# Make another API request to get video statistics
video_stats = youtube.videos().list(
    part=' snippet, statistics, contentDetails',
    id=','.join(video_ids),
    maxResults=num_videos
).execute()

# Extract the desired information from the API response
video_data = []
for video in video_stats['items']:
    video_id = video.get('id')
    title = video['snippet'].get('title')
    description = video['snippet'].get('description')
    thumbnail_url = video['snippet']['thumbnails']['default'].get('url')
    channel_title = video['snippet'].get('channelTitle')
    channel_id = video['snippet'].get('channelId')
    tags = video['snippet'].get('tags')
    default_audio_language = video['snippet'].get('defaultAudioLanguage')
    can_comment = video['snippet'].get('canComment')
    duration = video['contentDetails'].get('duration')
    content_rating = video['contentDetails'].get('contentRating')
    view_count = video['statistics'].get('viewCount')
    like_count = video['statistics'].get('likeCount')
    comment_count = video['statistics'].get('commentCount')
    video_data.append([video_id, title,
                       # description,
                       thumbnail_url, channel_title, channel_id, tags, default_audio_language, duration, content_rating, view_count, like_count, comment_count])

# Print the video IDs and titles
videos_column_names = ["video_id", "title",
                       # "description",
                       "thumbnail_url", "channel_title", "channel_id", "tags", "default_audio_language", "duration", "content_rating", "view_count", "like_count", "comment_count"]
comments_column_names = ['video_id', 'comment', 'like_count', 'reply_count']


videos_df = pd.DataFrame(video_data, columns=videos_column_names)
comments_df = pd.DataFrame(columns=comments_column_names)
for video_id in videos_df.loc[videos_df['comment_count'].notnull(), 'video_id']:
    temp = get_comments(video_id, num_comments)
    comments_df = comments_df.append(temp)

# write dataframe to csv file in extracted folder
videos_df.to_csv(f'{path_to_extracted}/videos.csv', index=False)
comments_df.to_csv(f'{path_to_extracted}/comments.csv', index=False)
