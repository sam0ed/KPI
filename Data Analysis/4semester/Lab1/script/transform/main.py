from cleaner_functions import *
from InputSource import InputSource

def main():
    channel_info_1=InputSource(r'C:\Users\vikto\GitRepos\KPI\Data Analysis\4semester\Lab1\channel_info',
                                r'most_subscribed_youtube_channels.csv',
                                  r'C:\Users\vikto\GitRepos\KPI\Data Analysis\4semester\Lab1\channel_info',
                                    r'cleaned_most_subscribed_youtube_channels.csv' )
    cahnnel_info_2=InputSource(r'C:\Users\vikto\GitRepos\KPI\Data Analysis\4semester\Lab1\channel_info',
                                r'most_subscribed_youtube_channels.csv',
                                  r'C:\Users\vikto\GitRepos\KPI\Data Analysis\4semester\Lab1\channel_info',
                                    r'cleaned_most_subscribed_youtube_channels.csv' )

    channel_info_1.create_destination()
    
    channel_info_1.add_clearing_function((lambda: remove_commas_from_numeric_csv(channel_info_1.get_full_dest_path()),
                                          lambda: remove_empty_rows_csv(channel_info_1.get_full_dest_path()),
                                          lambda: remove_column_csv(channel_info_1.get_full_dest_path(), column_to_drop='rank'))) # type: ignore
    
    channel_info_1.clear_destination()

if __name__ == "__main__":
    main()
