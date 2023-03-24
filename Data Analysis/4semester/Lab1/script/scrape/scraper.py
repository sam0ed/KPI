import random
import urllib.parse as urlparse
from urllib.parse import parse_qs
from selenium import webdriver
from selenium.webdriver.chrome.options import Options
from selenium.webdriver.support.ui import WebDriverWait
from selenium.common.exceptions import TimeoutException
from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.common.by import By
import time


class ElementsLengthChanges(object):
    def __init__(self, selector):
        self.selector = selector
        self.initial_length = None
        self.driver = None

    def __call__(self, driver):
        self.driver = driver
        self.set_initial_length()
        return (self.count_elements() > self.initial_length and self.count_elements()<100)

    def set_initial_length(self):
        if self.initial_length is None:
            self.initial_length = self.count_elements()

    def count_elements(self):
        return len(self.driver.find_elements_by_css_selector(self.selector))


def build_browser(proxy_ip=None):
    capabilities = webdriver.DesiredCapabilities.CHROME
    chrome_options = Options()
    chrome_options.add_argument('--disable-dev-shm-usage')
    chrome_options.add_argument("--disable-notifications")
    chrome_options.add_argument('--no-sandbox')
    chrome_options.add_argument('--verbose')
    chrome_options.add_argument('--disable-gpu')
    chrome_options.add_argument('--disable-software-rasterizer')
    chrome_options.add_argument('--headless')
    #
    # if proxy_ip:
    #     chrome_options.add_argument(f'--proxy-server=https://{proxy_ip}')
    #     webdriver.DesiredCapabilities.CHROME['proxy'] = {
    #         "httpProxy": proxy_ip,
    #         "ftpProxy": proxy_ip,
    #         "sslProxy": proxy_ip,
    #         "proxyType": "MANUAL",
    #
    #     }
    #     webdriver.DesiredCapabilities.CHROME['acceptSslCerts'] = True

    return webdriver.Chrome(r'C:\Users\vikto\Downloads\Apps\chrome_driver_unpacked\chromedriver.exe',
        desired_capabilities=capabilities,
        chrome_options=chrome_options
    )


def extract_video_id(url):
    parsed = urlparse.urlparse(url)
    return parse_qs(parsed.query)['v'][0]


def url_to_thumbnail(url):
    video_id = extract_video_id(url)
    return f"https://i3.ytimg.com/vi/{video_id}/hqdefault.jpg"

VIDEOS_SELECTOR=r'a#video-title-link'
MONETIZED_SELECTOR=r'[id^="ad-text"]'
PUBLISHED_DATE_SELECTOR=r'#info > span:nth-child(3)'
DESCRIPTION_SELECTOR=r'#expand'
VIDEO_TITLE_SELECTOR=r'#title > h1 > yt-formatted-string'
PAUSE_BUTTON_SELECTOR=r'#movie_player > div.ytp-chrome-bottom > div.ytp-chrome-controls > div.ytp-left-controls > button'
MOST_POPULAR_VIDEOS_SELECTOR='#chips > yt-chip-cloud-chip-renderer:nth-child(2)'

class YoutubeScraper:
    def __init__(self, channel_url, stop_id=None, max_videos=None, proxy_ip=None):
        self.channel_url = channel_url
        self.browser = build_browser(proxy_ip=proxy_ip)
        self.stop_id = stop_id
        self.max_videos = max_videos

    def __del__(self):
        self.browser.quit()

    def scrape_video_data(self):
        print(self.channel_url)

        self.navigate_to_videos()
        self.load_all_pages()

        print("Extracting video urls...")
        video_urls = self.extract_video_urls()

        scraped_videos = []
        for url in video_urls:
            print(url)

            self.navigate_to_video(url)
            scraped_videos.append(self.extract_video_attributes(url))

        # return scraped_videos
        return scraped_videos

    def extract_video_urls(self):
        urls = []

        video_elements=self.find_video_elements()
        for element in video_elements:
            url=element.get_attribute('href')
            if self.max_videos<len(urls):
                break

            urls.append(url)

        return urls

    def extract_video_attributes(self, url):
        pause_button=self.browser.find_element_by_css_selector(PAUSE_BUTTON_SELECTOR)
        time.sleep(random.randint(0,2))
        pause_button.click()
        add_elements= self.browser.find_elements_by_css_selector(MONETIZED_SELECTOR)
        time.sleep(random.randint(0, 2))
        monetization_status=bool(add_elements)
        pause_button.click()

        self.browser.find_element_by_css_selector(DESCRIPTION_SELECTOR).click()
        time.sleep(random.randint(0, 1))
        published_date = self.browser.find_element_by_css_selector(PUBLISHED_DATE_SELECTOR).text

        video_id = extract_video_id(url)

        return {
            "id": video_id,
            "published_date": published_date,
            "monetization_status": monetization_status
        }

    def count_video_elements(self):
        return len(self.find_video_elements())

    def find_video_elements(self):
        return self.browser.find_elements_by_css_selector(VIDEOS_SELECTOR)

    def navigate_to_videos(self):
        videos_url = f'{self.channel_url}/videos'
        self.browser.get(videos_url)
        self.accept_cookies()
        self.browser.find_element_by_css_selector(MOST_POPULAR_VIDEOS_SELECTOR).click()
        # try:
        #     WebDriverWait(self.browser, 10).until(EC.element_to_be_clickable((By.CSS_SELECTOR, 'button')))
        #     video=self.browser.find_element_by_css_selector('button')
        #     video.click()
        # except TimeoutException:
        #     print("Prepage doesn't exist")

    def navigate_to_video(self, url):
        self.browser.get(url)
        WebDriverWait(self.browser, 10).until(
            EC.presence_of_element_located((By.CSS_SELECTOR, VIDEO_TITLE_SELECTOR))
        )

    def load_all_pages(self):
        while True:
            try:
                self.load_next_page()
                print(self.count_video_elements())
            except TimeoutException:
                break

    def load_next_page(self):
        self.browser.execute_script("window.scrollTo(0, Math.max(document.body.scrollHeight, document.body.offsetHeight, document.documentElement.clientHeight, document.documentElement.scrollHeight, document.documentElement.offsetHeight))")
        WebDriverWait(self.browser, 10).until(ElementsLengthChanges(VIDEOS_SELECTOR))

    def is_stop_url(self, url):
        video_id = extract_video_id(url)
        return video_id == self.stop_id

    def accept_cookies(self):
        self.browser.find_element_by_xpath('//*[@id="yDmH0d"]/c-wiz/div/div/div/div[2]/div[1]/div[3]/div[1]/form[2]/div/div/button/span').click()
