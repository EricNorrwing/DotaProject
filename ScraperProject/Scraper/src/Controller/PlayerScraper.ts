import puppeteer from "puppeteer-extra";
import StealthPlugin from "puppeteer-extra-plugin-stealth";
import { Browser, executablePath } from "puppeteer";
import { Player } from "../model/Player";
import { PlayerExtractor } from "../extractor/player/PlayerExtractor";
import playerInfoBoxExtractor from "../extractor/player/PlayerInfoBoxExtractor";
import { updatePlayer } from "../util/player/playerUtil";

puppeteer.use(StealthPlugin());

const playerScraper = async (fullPlayerURL: string): Promise<Player> => {
  const browser: Browser = await puppeteer.launch({
    headless: true,
    executablePath: executablePath(),
  });

  let player: Player = {
    inGameName: "",
    name: "",
    nationality: "",
    earnings: 0,
    dateOfBirth: "",
    region: "",
    yearsActive: "",
    currentRoles: [],
    team: "",
    url: "",
  };

  try {
    const page = await browser.newPage();
    await page.goto(fullPlayerURL);

    player.inGameName = await page.$eval(
        PlayerExtractor.inGameNameExtractor,
        (el) => el.textContent?.trim() || ""
    );

    const partialPlayer: Player = await playerInfoBoxExtractor(page, player);

    player.name = partialPlayer.name;

    updatePlayer(player, partialPlayer);
  } catch (error) {
    throw new Error("PlayerScraper has failed: " + error);
  } finally {
    await browser.close();
  }

  return player;
};

export default playerScraper;
