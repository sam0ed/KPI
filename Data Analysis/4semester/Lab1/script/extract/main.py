from .ExtractConfig import ExtractConfig as config
from .EnvManager import EnvManager
from .YouTubeApi import YouTubeApi
from .YouTubeApiUtils import YouTubeApiUtils

def main() -> None:
    
    EnvManager.set_up(config.path_to_extracted)

    api_key=EnvManager.get_api_key(config.path_to_api_keys, config.key_owner, config.key_name)
    you_tube_api_utils=YouTubeApiUtils(api_key)
    you_tube_api_utils.print_available_video_attr_names()

    you_tube_api=YouTubeApi( api_key)

    comments_df=you_tube_api.get_comments(you_tube_api_utils._DUMMY_VIDEO_ID, config.num_comments, [attr_name for i, attr_name in enumerate(you_tube_api_utils.get_available_comment_attr_names()) if i in config.comment_attr_indexes])
    videos_df=you_tube_api.get_videos(you_tube_api_utils._DUMMY_CHANNEL_ID, config.num_videos, [attr_name for i, attr_name in enumerate(you_tube_api_utils.get_available_video_attr_names()) if i in config.video_attr_indexes])
    
    videos_df.to_csv(f'{config.path_to_extracted}/videos.csv', index=False)
    comments_df.to_csv(f'{config.path_to_extracted}/comments.csv', index=False)


if __name__=='__main__':
    main()