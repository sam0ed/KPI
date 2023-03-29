import pandas as pd

from .ExtractConfig import ExtractConfig as config
from .EnvManager import EnvManager
from .YouTubeApi import YouTubeApi
from .YouTubeApiUtils import YouTubeApiUtils


def main() -> None:
    ########## ADDITIONAL CONFIGURATION CODE ##########
    # you_tube_api_utils = YouTubeApiUtils(api_key)
    # you_tube_api_utils.print_available_video_attr_names()
    # with open(config.path_to_cache + '\\' + 'available_video_attr_names.csv', 'w') as csv_file:
    #     available_video_attr_names = you_tube_api_utils.get_available_video_attr_names()
    #     csv_file.writelines(line + '\n' for line in available_video_attr_names)
    # with open(config.path_to_cache + '\\' + 'available_comment_attr_names.csv', 'w') as csv_file:
    #     available_comment_attr_names = you_tube_api_utils.get_available_comment_attr_names()
    #     csv_file.writelines(line + '\n' for line in available_comment_attr_names)
    ########## ADDITIONAL CONFIGURATION CODE ##########

    EnvManager.set_up(config.path_to_extracted)
    api_key = EnvManager.get_api_key(config.path_to_api_keys, config.key_owner, config.key_name)
    you_tube_api = YouTubeApi(api_key)

    videos_ids = pd.read_csv(config.path_to_cleaned + '\\' + 'transformed_video_data.csv', header=None,
                             names=['id', 'publishing_date'])
    with open(config.path_to_cache + '\\' + 'available_video_attr_names.csv', 'r') as csv_file:
        available_video_attr_names = [line.strip() for line in csv_file.readlines()]
    videos_df = pd.DataFrame()
    videos_df = pd.concat([videos_df, you_tube_api.get_videos(videos_ids.id, [attr_name for i, attr_name in
                                                                              enumerate(available_video_attr_names)
                                                                              if i in config.video_attr_indexes])])
    videos_df.to_csv(f'{config.path_to_extracted}/videos.csv', index=False)

    comments_df = pd.DataFrame()
    with open(config.path_to_cache + '\\' + 'available_comment_attr_names.csv', 'r') as csv_file:
        available_comment_attr_names = [line.strip() for line in csv_file.readlines()]

    videos = pd.read_csv(config.path_to_extracted + '\\' + 'videos.csv')
    for video_index, video in videos.iterrows():
        try:
            comments_df = pd.concat([comments_df, you_tube_api.get_comments(video['id'], config.num_comments_per_video,
                                                                            [attr_name for i, attr_name in
                                                                             enumerate(
                                                                                 available_comment_attr_names)
                                                                             if
                                                                             i in config.comment_attr_indexes])],
                                    ignore_index=True)
            print(f'Video {video_index} has comments')
        except:
            print(f'Video {video_index} has !!!DISABLED!!! comments')

    comments_df.to_csv(f'{config.path_to_extracted}/comments.csv', index=False)


if __name__ == '__main__':
    main()
