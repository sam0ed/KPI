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