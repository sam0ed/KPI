from .cleaner_functions import *
from .InputSource import InputSource
from .TransformConfig import TransformConfig as config
import os


def main():
    transform_config = config(os.path.abspath(os.path.join(os.path.dirname(__file__), '../..', 'data/dirty')),
                              os.path.abspath(os.path.join(os.path.dirname(__file__), '../..', 'data/cleaned')))
    transform_config.set_up(transform_config.path_to_source)
    transform_config.set_up(transform_config.path_to_dest)

    # source_name_1 = r'most_subscribed_youtube_channels.csv'
    # channel_info_1 = InputSource(transform_config.path_to_source,
    #                              source_name_1,
    #                              transform_config.path_to_dest,
    #                              transform_config.dest_prefix + source_name_1)
    # channel_info_1.create_destination()
    # channel_info_1.add_transforming_function({lambda df: df.applymap(remove_commas),
    #                                          lambda df: df.dropna(),
    #                                          lambda df: df.drop(columns=['rank'])})
    # channel_info_1.transform_destination()

    source_name_2 = 'top_1000_youtubers.csv'
    channel_info_2 = InputSource(transform_config.path_to_source,
                                 source_name_2,
                                 transform_config.path_to_dest,
                                 transform_config.dest_prefix + source_name_2)
    channel_info_2.create_destination()
    channel_info_2.add_transforming_function({
        lambda df: df.dropna(),
        lambda df: df[~(df == "'N/A'").any(axis=1)],
        lambda df: df.drop(columns=['Rank'], inplace=True),
        lambda df: pd.concat([df.drop(columns=['Subscribers', 'Avg. Views', 'Avg. Likes', 'Avg. Comments']),
                              df.loc[:, ['Subscribers', 'Avg. Views', 'Avg. Likes', 'Avg. Comments']].applymap(
                                  remove_abbreviation)], axis=1)
    })
    channel_info_2.transform_destination()


if __name__ == "__main__":
    main()
    # channel_info_1.add_transforming_function(
    #     (lambda: remove_commas_from_numeric_csv(channel_info_1.get_full_dest_path()),
    #      lambda: remove_empty_rows_csv(channel_info_1.get_full_dest_path()),
    #      lambda: remove_column_csv(channel_info_1.get_full_dest_path(), column_to_drop='rank')))
