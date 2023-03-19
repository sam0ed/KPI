import json
import os
import utils
from ConfigProgram import ConfigProgram
from EnvManager import EnvManager
from YouTubeApi import YouTubeApi
from YouTubeApiUtils import YouTubeApiUtils
from googleapiclient.discovery import build
import pandas as pd



def main() -> None:
    env=EnvManager(path_to_api_keys=r'G:\My Drive\api_keys.ini',
    key_owner='Google',
    key_name='api_key_1',
    path_to_extracted=os.path.abspath(os.path.join(os.path.dirname(__file__), '../..', 'data/extracted')) )

    you_tube_api_utils=YouTubeApiUtils(env.get_api_key())
    # you_tube_api_utils.print_available_video_attr_names()

    you_tube_api=YouTubeApi( env.get_api_key())
    comments_df=you_tube_api.get_comments(you_tube_api_utils._DUMMY_VIDEO_ID, ConfigProgram.num_comments, [attr_name for i, attr_name in enumerate(you_tube_api_utils.get_available_comment_attr_names()) if i in ConfigProgram.comment_attr_indexes])
    videos_df=you_tube_api.get_videos(you_tube_api_utils._DUMMY_CHANNEL_ID, ConfigProgram.num_videos, [attr_name for i, attr_name in enumerate(you_tube_api_utils.get_available_video_attr_names()) if i in ConfigProgram.video_attr_indexes])
    
    videos_df.to_csv(f'{env.path_to_extracted}/videos.csv', index=False)
    comments_df.to_csv(f'{env.path_to_extracted}/comments.csv', index=False)
    print('end')
    
    
 

if __name__=='__main__':
    main()