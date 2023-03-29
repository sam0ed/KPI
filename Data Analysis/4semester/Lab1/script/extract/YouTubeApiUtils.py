from googleapiclient.discovery import build
from .ExtractConfig import ExtractConfig as config
from .utils import *


class YouTubeApiUtils:
    def __init__(self, api_key: str) -> None:
        # constants
        self._DUMMY_VIDEO_ID = 'PHgc8Q6qTjc'
        self._DUMMY_CHANNEL_ID = 'UC-lHJZR3Gqxm24_Vd_AJ5Yw'
        self._DUMMY_PAGE_TOKEN = None
        self._MAX_RESULTS = 1

        # resource
        self._RESOURCE = build('youtube', 'v3', developerKey=api_key)

    # cost=1
    def print_available_comment_attr_names(self):
        print('AVAILABLE COMMENT ATTRIBUTE NAMES')
        print_json_attr_names(self._RESOURCE.commentThreads().list(
            part=[value for i, value in enumerate(config.available_comment_part_values) if
                  i in config.comment_part_indexes],
            videoId=self._DUMMY_VIDEO_ID,
            maxResults=self._MAX_RESULTS,
            pageToken=self._DUMMY_PAGE_TOKEN
        ).execute())
        print('----------------------------------------')

    # cost=1
    def get_available_comment_attr_names(self):
        return get_json_attr_names(self._RESOURCE.commentThreads().list(
            part=[value for i, value in enumerate(config.available_comment_part_values) if
                  i in config.comment_part_indexes],
            videoId=self._DUMMY_VIDEO_ID,
            maxResults=self._MAX_RESULTS,
            pageToken=self._DUMMY_PAGE_TOKEN
        ).execute())

    # cost=100
    def get_videos_ids_by_channel(self):
        videos = self._RESOURCE.search().list(
            part='id',
            channelId=self._DUMMY_CHANNEL_ID,
            type='video',
            order='viewCount',
            maxResults=self._MAX_RESULTS
        ).execute()

        # Extract the video IDs from the API response
        video_ids = [video['id']['videoId'] for video in videos['items']]

        return video_ids

    # cost=1
    def print_available_video_attr_names(self):
        video_ids = self.get_videos_ids_by_channel()

        # Make another API request to get video statistics
        print('AVAILABLE VIDEO ATTRIBUTE NAMES')
        print_json_attr_names(self._RESOURCE.videos().list(
            part=[value for i, value in enumerate(config.available_video_part_values) if
                  i in config.video_part_indexes],
            id=','.join(video_ids),
            maxResults=self._MAX_RESULTS
        ).execute())
        print('----------------------------------------')

    # cost=1
    def get_available_video_attr_names(self):
        video_ids = self.get_videos_ids_by_channel()

        # Make another API request to get video statistics
        return get_json_attr_names(self._RESOURCE.videos().list(
            part=[value for i, value in enumerate(config.available_video_part_values) if
                  i in config.video_part_indexes],
            id=','.join(video_ids),
            maxResults=self._MAX_RESULTS
        ).execute())
