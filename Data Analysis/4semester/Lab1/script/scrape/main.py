import time
import random

import pandas as pd

from script.scrape.BrowserManager import BrowserManager
from script.scrape.ScraperConfig import ScraperConfig
from script.scrape.YoutubeScraper import YoutubeScraper


def main() -> None:
    channels_df = pd.read_csv(f'{ScraperConfig.path_to_cleaned}/merged_channel_data.csv')
    video_data = []
    BrowserManager.clear_proxy_list()
    with open(ScraperConfig.working_http_proxies, 'r') as proxy_list_file:
        proxy_list: list[str] = [proxy.strip() for proxy in proxy_list_file.readlines()]

    amount_of_channels = len(channels_df)
    amount_of_channels_processed = 270

    amount_of_requests_for_cur_proxy = ScraperConfig.max_requests_on_one_proxy
    cur_proxy_index = 0
    scraper = YoutubeScraper(max_videos=ScraperConfig.max_videos)
    checked_proxies_in_row =0
    try:
        while amount_of_channels_processed < amount_of_channels:
            while (amount_of_requests_for_cur_proxy >= ScraperConfig.max_requests_on_one_proxy and amount_of_requests_for_cur_proxy != 0 and checked_proxies_in_row != len(proxy_list)):
                scraper.proxy_ip = proxy_list[cur_proxy_index % len(proxy_list)]
                if BrowserManager.is_proxy_working(scraper.proxy_ip):
                    amount_of_requests_for_cur_proxy = 0
                    checked_proxies_in_row =0
                else:
                    cur_proxy_index += 1
                    checked_proxies_in_row+=1
            if checked_proxies_in_row>=len(proxy_list):
                break
            scraper.browser = BrowserManager.build_browser(enable_ui=True, proxy_ip=scraper.proxy_ip)
            scraper.channel_url = 'https://www.youtube.com/channel/' + channels_df.loc[amount_of_channels_processed, 'ID']
            try:
                channel_videos_data = scraper.scrape_video_data()
            except:
                amount_of_requests_for_cur_proxy = ScraperConfig.max_requests_on_one_proxy
                cur_proxy_index += 1
                scraper.browser.quit()
                continue
            if channel_videos_data is not None:
                video_data.extend(channel_videos_data)
                amount_of_requests_for_cur_proxy += len(channel_videos_data)

            amount_of_channels_processed += 1
            scraper.browser.quit()

            if amount_of_channels_processed %3==0 :
                videos_df = pd.DataFrame(video_data)
                videos_df.to_csv(f'{ScraperConfig.path_to_scraped}/video_data.csv',mode='a', index=False, header=False)
                video_data = []
            print(f'amount_of_channels_processed:{amount_of_channels_processed} cur_proxy_index:{cur_proxy_index}')

    except Exception as e:
        print(e)
    finally:
        videos_df = pd.DataFrame(video_data)
        videos_df.to_csv(f'{ScraperConfig.path_to_scraped}/video_data.csv', mode='a', index=False, header=False)
        print(f'amount_of_channels_processed:{amount_of_channels_processed} cur_proxy_index:{cur_proxy_index}')


if __name__ == '__main__':
    main()
