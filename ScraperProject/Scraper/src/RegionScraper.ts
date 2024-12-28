import puppeteer from 'puppeteer-extra';
import StealthPlugin from 'puppeteer-extra-plugin-stealth';
import { Browser, ElementHandle, executablePath } from 'puppeteer';

puppeteer.use(StealthPlugin());

const regionScraper = async (scrapeURL: string, selectors: string[]): Promise<Set<string>> => {
  const browser: Browser = await puppeteer.launch({
    headless: true,
    executablePath: executablePath()
  });

  const listOfRegions: Set<string> = new Set();

  try {
    const page = await browser.newPage();
    await page.goto(scrapeURL);

    for (const selector of selectors) {
      const pageElements: ElementHandle<Element>[] = await page.$$(selector);

      const data = await Promise.all(
        pageElements.map(async (item) => {
          return item.evaluate(el => el.textContent?.trim() || '');
        })
      );

      const cleanedData = data.filter((item): item is string => item !== '');
      cleanedData.forEach(item => listOfRegions.add(item));
    }

    console.log([...listOfRegions]);
    return listOfRegions;
  } catch (error) {
    console.error('Error during scraping:', error);
    return new Set();
  } finally {
    await browser.close();
  }
};

export default regionScraper;
