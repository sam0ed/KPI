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
    def __init__(self, channel_url, stop_id=None, max_videos=None, proxy_ip=None):
        self.channel_url = channel_url
        self.browser = BrowserManager.build_browser(enable_ui=False, proxy_ip=proxy_ip)
        self.stop_id = stop_id
        self.max_videos = max_videos

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
        pause_button=self.browser.find_element_by_css_selector(ScraperConfig.PAUSE_BUTTON_SELECTOR)
        time.sleep(random.randint(0,2))
        pause_button.click()
        add_elements= self.browser.find_elements_by_css_selector(ScraperConfig.MONETIZED_SELECTOR)
        time.sleep(random.randint(0, 2))
        monetization_status=bool(add_elements)
        pause_button.click()

        self.browser.find_element_by_css_selector(ScraperConfig.DESCRIPTION_SELECTOR).click()
        time.sleep(random.randint(0, 1))
        published_date = self.browser.find_element_by_css_selector(ScraperConfig.PUBLISHED_DATE_SELECTOR).text

        video_id = extract_video_id(url)

        return {
            "id": video_id,
            "published_date": published_date,
            "monetization_status": monetization_status
        }

    def count_video_elements(self):
        return len(self.find_video_elements())

    def find_video_elements(self):
        return self.browser.find_elements_by_css_selector(ScraperConfig.VIDEOS_SELECTOR)

    def navigate_to_videos(self):
        videos_url = f'{self.channel_url}/videos'
        self.browser.get(videos_url)
        self.accept_cookies()
        self.browser.find_element_by_css_selector(ScraperConfig.MOST_POPULAR_VIDEOS_SELECTOR).click()

    def navigate_to_video(self, url):
        self.browser.get(url)
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
        WebDriverWait(self.browser, 10).until(ElementsLengthChanges(ScraperConfig.VIDEOS_SELECTOR))

    def accept_cookies(self):
        self.browser.find_element_by_xpath('//*[@id="yDmH0d"]/c-wiz/div/div/div/div[2]/div[1]/div[3]/div[1]/form[2]/div/div/button/span').click()

    def perform_random_mouse_action(self):
        # Get the browser window size
        window_size = self.browser.execute_script("return [window.innerWidth, window.innerHeight]")
        time.sleep(random.randint(0, 2))
        # # Generate a random mouse position within the window
        x = random.randint(0, window_size[0])
        y = random.randint(0, window_size[1])

        # Move the mouse to the random position and perform a random action
        action_chains = ActionChains(self.browser)
        action_chains.move_by_offset(x, y)
        # if random.random() < 0.5:
        #     # Move the mouse in a random direction
        #     direction = random.choice(['up', 'down', 'left', 'right'])
        #     if direction == 'up':
        #         action_chains.move_by_offset(0, -10)
        #     elif direction == 'down':
        #         action_chains.move_by_offset(0, 10)
        #     elif direction == 'left':
        #         action_chains.move_by_offset(-10, 0)
        #     elif direction == 'right':
        #         action_chains.move_by_offset(10, 0)
        # else:
            # Scroll the page
        scroll_amount = random.randint(0, window_size[1])
        self.browser.execute_script(f"window.scrollBy(0, {scroll_amount})")
        time.sleep(random.randint(ScraperConfig.max_random_await_in_seconds, ScraperConfig.max_random_await_in_seconds ))
        action_chains.perform()


