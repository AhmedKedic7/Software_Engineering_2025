const { buildDriver } = require("./driver");
const { By, until } = require("selenium-webdriver");

(async function registerTest() {
  const driver = await buildDriver();

  try {
    await driver.get("https://shoestoreprasaikedi.netlify.app/register");
    console.log("Page loaded");

    await driver
      .findElement(By.css('input[name="firstName"]'))
      .sendKeys("Emir");

    await driver.findElement(By.css('input[name="lastName"]')).sendKeys("Kedi");

    await driver
      .findElement(By.css('input[name="email"]'))
      .sendKeys("eemaemir@gmail.com");

    await driver
      .findElement(By.css('input[name="password"]'))
      .sendKeys("emir1234");

    await driver
      .findElement(By.css('input[name="confirmPassword"]'))
      .sendKeys("emir1234");

    await driver
      .findElement(By.css('input[name="phoneNumber"]'))
      .sendKeys("062760286");

    await driver.findElement(By.css('button[type="submit"]')).click();

    console.log("Register form submitted");
  } catch (err) {
    console.error("Register test failed:", err);
  } finally {
    await driver.quit();
  }
})();
