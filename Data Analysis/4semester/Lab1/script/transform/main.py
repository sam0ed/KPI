import pandas as pd
import os
from .cleaner_functions import *
from .InputSource import InputSource
from .TransformConfig import TransformConfig as config


def main():
    dirty_config = config(os.path.abspath(os.path.join(os.path.dirname(__file__), '../..', 'data/dirty')),
                              os.path.abspath(os.path.join(os.path.dirname(__file__), '../..', 'data/cleaned')))
    dirty_config.set_up(dirty_config.path_to_dest)
    scraped_config = config(
        os.path.abspath(os.path.join(os.path.dirname(__file__), '../..', 'data/scraped')),
        os.path.abspath(os.path.join(os.path.dirname(__file__), '../..', 'data/cleaned')))
    scraped_config.set_up(scraped_config.path_to_dest)
    extracted_config = config(
        os.path.abspath(os.path.join(os.path.dirname(__file__), '../..', 'data/extracted')),
        os.path.abspath(os.path.join(os.path.dirname(__file__), '../..', 'data/cleaned')))
    extracted_config.set_up(extracted_config.path_to_dest)

    source_name_1 = r'most_subscribed_youtube_channels.csv'
    channel_info_1 = InputSource(dirty_config.path_to_source,
                                 source_name_1,
                                 dirty_config.path_to_dest,
                                 dirty_config.dest_prefix + source_name_1)
    channel_info_1.create_destination()
    channel_info_1.add_transforming_function({lambda df: df.applymap(remove_commas),
                                              lambda df: df.dropna(),
                                              lambda df: df.drop(columns=['rank'])})
    channel_info_1.transform_destination()

    source_name_2 = 'top_1000_youtubers.csv'
    channel_info_2 = InputSource(dirty_config.path_to_source,
                                 source_name_2,
                                 dirty_config.path_to_dest,
                                 dirty_config.dest_prefix + source_name_2)
    channel_info_2.create_destination()
    channel_info_2.add_transforming_function({
        lambda df: df.drop(columns=['Rank'], inplace=True),
        lambda df: pd.concat([df.drop(columns=['Subscribers', 'Avg. Views', 'Avg. Likes', 'Avg. Comments']),
                              df.loc[:, ['Subscribers', 'Avg. Views', 'Avg. Likes', 'Avg. Comments']].applymap(
                                  remove_abbreviation)], axis=1),
        lambda df: df.dropna()
    })
    channel_info_2.transform_destination()

    video_name_1 = 'video_data.csv'
    video_info_1 = InputSource(scraped_config.path_to_source,
                                 video_name_1,
                                 scraped_config.path_to_dest,
                                 scraped_config.dest_prefix + video_name_1)
    video_info_1.create_destination()
    df = pd.read_csv(video_info_1.get_full_dest_path(), header=None)
    df = df.drop_duplicates()
    df.iloc[:, 1] = df.iloc[:, 1].apply(lambda x: x.replace('Premiered', '').strip())
    df.iloc[:, 1] = df.iloc[:, 1].apply(transform_date_to_iso)
    df.to_csv(video_info_1.get_full_dest_path(), index=False)


###################!!!MERGER!!!####################
    merge_source_1 = pd.read_csv(channel_info_1.get_full_dest_path(),
                                 usecols=['Youtuber', 'subscribers', 'video views', 'video count', 'category',
                                          'started'])
    merge_source_2 = pd.read_csv(channel_info_2.get_full_dest_path(),
                                 usecols=['Name', 'username', 'Youtube Url', 'Audience Country', 'Avg. Likes',
                                          'Avg. Comments'])
    merge_source_3 = pd.read_csv(dirty_config.path_to_source + '\\' + 'top_channel_id.csv')

    merge_dest_1 = pd.merge(merge_source_1, merge_source_2, left_on='Youtuber', right_on='Name')
    merge_dest_1 = merge_dest_1.drop('Name', axis=1)
    merge_dest_1 = pd.merge(merge_dest_1, merge_source_3, left_on='Youtuber', right_on='Name')
    merge_dest_1 = merge_dest_1.drop('Name', axis=1)
    merge_dest_1.drop_duplicates(inplace=True)
    merge_dest_1.drop_duplicates(subset=['ID'],inplace=True)
    merge_dest_1 = merge_dest_1.dropna(subset=['Avg. Likes', 'Avg. Comments'])
    merge_dest_1.to_csv(dirty_config.path_to_dest + '\\' + r'merged_channel_data.csv', index=False)

    merge_source_4 = pd.read_csv(video_info_1.get_full_dest_path())
    merge_source_5 = pd.read_csv( extracted_config.path_to_source + '\\' + 'videos.csv')

    merge_dest_2 = pd.merge(merge_source_4, merge_source_5, left_on='0', right_on='id')
    merge_dest_2 = merge_dest_2.drop('0', axis=1)
    merge_dest_2.drop_duplicates(inplace=True)
    merge_dest_2.tags= merge_dest_2.tags.str.replace(r'^\[(.*)\]$', r'{\1}', regex=True)
    merge_dest_2=merge_dest_2.dropna(subset=['likeCount', 'commentCount'])
    merge_dest_2.to_csv(extracted_config.path_to_dest + '\\' + r'merged_video_data.csv', index=False)

    merge_source_6 = pd.read_csv(dirty_config.path_to_source + '\\' + r'world_population.csv', usecols=['Country','Population (2020)','Yearly Change','World Share'])
    merge_source_7 = pd.read_excel(dirty_config.path_to_source + '\\' + r'countriesViewers.xlsx', sheet_name='Data')
    merge_dest_3 = pd.merge(merge_source_6, merge_source_7,left_on='Country', right_on='country')
    merge_dest_3 = merge_dest_3.drop('Country', axis=1)
    merge_dest_3.drop_duplicates(inplace=True)
    merge_dest_3['users_amount']=(merge_dest_3['users_amount']*1000_000).astype(int)
    merge_dest_3.to_csv(dirty_config.path_to_dest + '\\' + r'merged_country_data.csv', index=False)
    print(merge_dest_1[~merge_dest_1['Audience Country'].isin(merge_dest_3.country)]['Audience Country'])
###################!!!MERGER!!!####################


    if __name__ == "__main__":
        main()
