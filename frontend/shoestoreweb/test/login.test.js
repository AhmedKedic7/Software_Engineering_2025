const { buildDriver } = require("./driver");
const { By, until } = require("selenium-webdriver");

function sleep(ms) {
  return new Promise((resolve) => setTimeout(resolve, ms));
}

(async function loginTest() {
  const driver = await buildDriver();

  try {
    await driver.get("https://shoestoreprasaikedi.netlify.app/login");
    await driver.wait(until.titleContains("Project1"), 5000);
    console.log("Page loaded");

    await driver
      .findElement(By.css('input[name="email"]'))
      .sendKeys("eemaemir@gmail.com");
    await driver
      .findElement(By.css('input[name="password"]'))
      .sendKeys("emir1234");
    await sleep(3000);
    await driver.findElement(By.css('button[type="submit"]')).click();

    await driver.wait(
      until.urlIs("https://shoestoreprasaikedi.netlify.app/"),
      5000
    );
    console.log("Login test passed");
  } catch (err) {
    console.error("Login test failed:", err);
  } finally {
    await driver.quit();
  }
})();
