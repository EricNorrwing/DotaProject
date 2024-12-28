import playerScraper from "./Controller/PlayerScraper";

const app = async () => {
  const fullPlayerURL = "https://liquipedia.net/dota2/MiCKe";
  let player = await playerScraper(fullPlayerURL);
  console.log(player);
};

app();