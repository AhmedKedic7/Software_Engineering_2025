const { buildDriver } = require("./driver");
const { By, until } = require("selenium-webdriver");

function sleep(ms) {
  return new Promise((resolve) => setTimeout(resolve, ms));
}

(async function loginTest() {
  const driver = await buildDriver();

  try {
    await driver.get("https://shoestoreprasaikedi.netlify.app");
    await driver.wait(until.titleContains("Project1"), 5000);
    console.log("Page loaded");

    await driver.executeScript("window.scrollTo(0, 600);");
    await sleep(500);

    const firstItem = await driver.findElement(
      By.xpath(
        "/html/body/app-root/div/app-home/section/div[2]/div/div[1]/app-shoe-card/div/div/div/div/a"
      )
    );

    firstItem.click();
    await sleep(500);

    const buttonAddToCart = await driver.findElement(
      By.xpath(
        "/html/body/app-root/div/app-product/div/div/div/div[2]/button[1]"
      )
    );

    buttonAddToCart.click();

    await driver.wait(
      until.elementLocated(By.css(".notification-container > .alert")),
      5000
    );

    await sleep(2000);
    console.log("Add to cart test passed");
  } catch (err) {
    console.error("Add to cart test failed:", err);
  } finally {
    await driver.quit();
  }
})();
