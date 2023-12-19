import os

class ScraperConfig:
    max_videos=10
    http_proxies=os.path.join(os.path.dirname(os.path.abspath(__file__)), 'proxy_list_files/http_proxies.txt')
    working_http_proxies=os.path.join(os.path.dirname(os.path.abspath(__file__)),
                                      'proxy_list_files/working_http_proxies.txt')
    max_requests_on_one_proxy=100
    batch_size=50

    min_random_await_in_seconds=0
    max_random_await_in_seconds=2

    VIDEOS_SELECTOR = r'a#video-title-link'
    MONETIZED_SELECTOR = r'[id^="ad-text"]'
    PUBLISHED_DATE_SELECTOR = r'#info > span:nth-child(3)'
    DESCRIPTION_SELECTOR = r'#expand'
    VIDEO_TITLE_SELECTOR = r'#title > h1 > yt-formatted-string'
    PAUSE_BUTTON_SELECTOR = r'#movie_player > div.ytp-chrome-bottom > div.ytp-chrome-controls > div.ytp-left-controls > button'
    MOST_POPULAR_VIDEOS_SELECTOR = '#chips > yt-chip-cloud-chip-renderer:nth-child(2)'
    MUTE_BUTTON_SELECTOR = r'#movie_player > div.ytp-chrome-bottom > div.ytp-chrome-controls > div.ytp-left-controls > span > button'

    path_to_cleaned=os.path.abspath(os.path.join(os.path.dirname(__file__), '../..', 'data/cleaned'))
    path_to_scraped=os.path.abspath(os.path.join(os.path.dirname(__file__), '../..', 'data/scraped'))