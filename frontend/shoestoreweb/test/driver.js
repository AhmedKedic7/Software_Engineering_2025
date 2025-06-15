const { Builder } = require("selenium-webdriver");
const chrome = require("selenium-webdriver/chrome");

async function buildDriver() {
  return await new Builder()
    .forBrowser("chrome")
    .setChromeOptions(new chrome.Options() /*.headless()*/)
    .build();
}

module.exports = { buildDriver };
