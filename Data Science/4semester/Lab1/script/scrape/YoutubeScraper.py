import random
import urllib.parse as urlparse
from urllib.parse import parse_qs

from selenium.webdriver import ActionChains
from selenium.webdriver.support.ui import WebDriverWait
from selenium.common.exceptions import TimeoutException
from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.common.by import By
import time

from script.scrape.ElementsLengthChanges import ElementsLengthChanges
from script.scrape.BrowserManager import BrowserManager
from script.scrape.ScraperConfig import ScraperConfig


def extract_video_id(url):
    parsed = urlparse.urlparse(url)
    return parse_qs(parsed.query)['v'][0]

class YoutubeScraper:
    def __init__(self, max_videos=None):
        self.channel_url = None
        self.proxy_ip = None
        self.browser = None
        self.max_videos = max_videos

    def scrape_video_data(self):
        print(self.channel_url)

        try:
            self.navigate_to_videos()
        except:
            return
        self.load_all_pages()

        print("Extracting video urls...")
        video_urls = self.extract_video_urls()

        scraped_videos = []
        for url in video_urls:
            print(url)
            self.navigate_to_video(url)
            # time.sleep(random.randint(1, 3))
            try:
                scraped_videos.append(self.extract_video_attributes(url))
            except:
                print("Failed to extract video attributes, skipping...")

        # return scraped_videos
        return scraped_videos

    def extract_video_urls(self):
        urls = []

        for element in self.find_video_elements():
            url=element.get_attribute('href')
            urls.append(url)
            if self.max_videos<=len(urls):
                break

        return urls

    def extract_video_attributes(self, url):
        pause_button=self.browser.find_element_by_css_selector(ScraperConfig.PAUSE_BUTTON_SELECTOR)
        time.sleep(random.randint(1,3))
        pause_button.click()
        # add_elements= self.browser.find_elements_by_css_selector(ScraperConfig.MONETIZED_SELECTOR)
        # time.sleep(random.randint(1, 3))
        # monetization_status=bool(add_elements)
        # pause_button.click()
        time.sleep(random.randint(1, 2))
        self.browser.find_element_by_css_selector(ScraperConfig.DESCRIPTION_SELECTOR).click()
        time.sleep(random.randint(1, 2))
        published_date = self.browser.find_element_by_css_selector(ScraperConfig.PUBLISHED_DATE_SELECTOR).text
        #
        # self.perform_random_mouse_action()

        return {
            "id": extract_video_id(url),
            "published_date": published_date
            # "monetization_status": monetization_status
        }

    def count_video_elements(self):
        return len(self.find_video_elements())

    def find_video_elements(self):
        return self.browser.find_elements_by_css_selector(ScraperConfig.VIDEOS_SELECTOR)

    def navigate_to_videos(self):
        videos_url = f'{self.channel_url}/videos'
        self.browser.get(videos_url)
        self.accept_cookies()
        # WebDriverWait(self.browser, 10).until(
        #     EC.presence_of_element_located((By.CSS_SELECTOR, ScraperConfig.MOST_POPULAR_VIDEOS_SELECTOR))
        # )
        time.sleep(random.randint(1, 3))
        self.browser.find_element_by_css_selector(ScraperConfig.MOST_POPULAR_VIDEOS_SELECTOR).click()
        time.sleep(1)

    def navigate_to_video(self, url):
        self.browser.get(url)
        # print(self.browser.page_source)
        # if self.browser.find_element_by_css_select('#recaptcha-anchor > div.recaptcha-checkbox-border'):
        #     print("Recaptcha detected")
        # else:
        #     print("Recaptcha not detected")
        WebDriverWait(self.browser, 10).until(
            EC.presence_of_element_located((By.CSS_SELECTOR, ScraperConfig.VIDEO_TITLE_SELECTOR))
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
        WebDriverWait(self.browser, 3).until(ElementsLengthChanges(ScraperConfig.VIDEOS_SELECTOR))

    def accept_cookies(self):
        self.browser.find_element_by_xpath('//*[@id="yDmH0d"]/c-wiz/div/div/div/div[2]/div[1]/div[3]/div[1]/form[2]/div/div/button/span').click()

    def perform_random_mouse_action(self):
        # Get the browser window size
        window_size = self.browser.execute_script("return [window.innerWidth, window.innerHeight]")
        time.sleep(random.randint(1, 2))

        if random.random() < 0.5:
            self.browser.find_element_by_css_selector(ScraperConfig.MUTE_BUTTON_SELECTOR).click()
            time.sleep(
                random.randint(ScraperConfig.max_random_await_in_seconds, ScraperConfig.max_random_await_in_seconds))

        # mouse_position = self.browser.execute_script(
        #     "return [window.scrollX + window.event.clientX, window.scrollY + window.event.clientY];")
        # # # Generate a random mouse position within the window
        # x = random.randint(-mouse_position[0], window_size[0]-mouse_position[0])
        # y = random.randint(-mouse_position[1], window_size[1]-mouse_position[1])
        #
        # # Move the mouse to the random position and perform a random action
        # action_chains.move_by_offset(x, y)
        # action_chains.perform()

        scroll_amount = random.randint(0, window_size[1])
        self.browser.execute_script(f"window.scrollBy(0, {scroll_amount})")
        time.sleep(random.randint(ScraperConfig.max_random_await_in_seconds, ScraperConfig.max_random_await_in_seconds ))
        scroll_amount = random.randint(- window_size[1], 0)
        self.browser.execute_script(f"window.scrollBy(0, {scroll_amount})")
        time.sleep(random.randint(ScraperConfig.max_random_await_in_seconds, ScraperConfig.max_random_await_in_seconds))


