import time

from selenium.webdriver.common.by import By
from selenium import webdriver
from selenium.webdriver.chrome.options import Options
from selenium.webdriver.support.wait import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC

from script.scrape.ScraperConfig import ScraperConfig


class BrowserManager:
    @staticmethod
    def is_proxy_working(browser, used_proxy):
        print(f"Testing proxy with ip {used_proxy}")
        try:
            browser.get('https://whatismyipaddress.com/de/meine-ip')
            print('Connection successful')
        except:
            print(f'Failed to connect to proxy')
            return False

        wait = WebDriverWait(browser, 5)
        accept_cookies_selector = r'#qc-cmp2-ui > div.qc-cmp2-footer.qc-cmp2-footer-overlay.qc-cmp2-footer-scrolled > div > button.css-47sehv'
        displayed_ip_address_selector = r'#ipv4 > a'
        try:
            wait.until(
                EC.presence_of_element_located((By.CSS_SELECTOR, accept_cookies_selector))
            )
            browser.find_element(By.CSS_SELECTOR, accept_cookies_selector).click()
        except:
            print(f'Failed to accept cookies')

        ip_address=None
        try:
            wait.until(EC.presence_of_element_located((By.CSS_SELECTOR, displayed_ip_address_selector)))
            # Get the IP address displayed on the page
            ip_address = browser.find_element(By.CSS_SELECTOR, displayed_ip_address_selector).text
        except:
            print(f'Failed to get IP address')

        # Compare the IP addresses
        return ip_address == used_proxy.split(':')[0]

    @staticmethod
    def build_browser(enable_ui:bool, proxy_ip=None):
        capabilities = webdriver.DesiredCapabilities.CHROME.copy()
        chrome_options = Options()
        chrome_options.add_argument('--disable-dev-shm-usage')
        chrome_options.add_argument("--disable-notifications")
        chrome_options.add_argument('--no-sandbox')
        chrome_options.add_argument('--verbose')
        chrome_options.add_argument('--disable-gpu')
        chrome_options.add_argument('--disable-software-rasterizer')
        if not enable_ui:
            print('UI disabled')
            chrome_options.add_argument('--headless')

        if proxy_ip:
            chrome_options.add_argument(f'--proxy-server=https://{proxy_ip}')
            capabilities['proxy'] = {
                "httpProxy": proxy_ip,
                "ftpProxy": proxy_ip,
                "sslProxy": proxy_ip,
                "proxyType": "MANUAL",

            }
            capabilities['acceptSslCerts'] = True

        return webdriver.Chrome(r'C:\Users\vikto\Downloads\Apps\chrome_driver_unpacked\chromedriver.exe',
                                desired_capabilities=capabilities,
                                chrome_options=chrome_options)

    @staticmethod
    def get_working_proxies(source: list[str]) -> list[str]:
        working_proxies: list[str] = list()
        for proxy in source:
            browser = BrowserManager.build_browser(enable_ui=True, proxy_ip=proxy)
            if BrowserManager.is_proxy_working(browser, proxy):
                working_proxies.append(proxy)
            browser.quit()
        return working_proxies

    @staticmethod
    def clear_proxy_list():
        with open(ScraperConfig.http_proxies, 'r') as proxies:
            current_proxy_ip: list[str] = [proxy.strip() for proxy in proxies.readlines()]
            working_proxies = [proxy+'\n' for proxy in BrowserManager.get_working_proxies(current_proxy_ip)]
        with open(ScraperConfig.working_http_proxies, 'w') as proxies:
            proxies.writelines(working_proxies)

    # @staticmethod
    # async def solve_challenge(page):
    #     # Wait for the spinner to disappear
    #     await page.waitForSelector('#challenge-spinner', {'hidden': True})
    #
    #     # Check if the challenge is displayed
    #     if await page.J('#challenge-form'):
    #         # Solve the challenge using pyppeteer
    #         await page.solveRecaptchas()
    #         await page.click('#challenge-form button[type="submit"]')
    #
    #         # Wait for the page to reload after solving the challenge
    #         await page.waitForNavigation()
    #
    #     return page
