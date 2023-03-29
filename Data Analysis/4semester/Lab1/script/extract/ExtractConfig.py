import os


class ExtractConfig:
    num_comments_per_video: int = 100

    # part parameter available values
    available_comment_part_values: list[str] = ['id', 'replies', 'snippet']
    available_video_part_values: list[str] = ['contentDetails', 'id', 'liveStreamingDetails', 'recordingDetails',
                                              'snippet', 'statistics', 'status', 'topicDetails']
    available_channel_part_values: list[str] = ["snippet", "contentDetails", "statistics", "status"]

    comment_attr_indexes = [12, 13, 15, 18, 21, 22, 25]
    video_attr_indexes = indexes = [4, 6, 7, 8, 9, 24, 25, 37, 42, 43, 45, 50, 51, 53]

    comment_part_indexes = [2]
    video_part_indexes = [0, 1, 4, 5]
    channel_part_values = [0, 1, 2, 3]

    path_to_api_keys = r'G:\My Drive\api_keys.ini'
    key_owner = 'Google'
    key_name = 'api_key_1'
    path_to_extracted = os.path.abspath(os.path.join(os.path.dirname(__file__), '../..', 'data/extracted'))
    path_to_cleaned = os.path.abspath(os.path.join(os.path.dirname(__file__), '../..', 'data/cleaned'))
    path_to_cache = os.path.abspath(os.path.join(os.path.dirname(__file__), '../..', 'data/cache'))
