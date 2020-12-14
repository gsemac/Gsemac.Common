// https://github.com/berstend/puppeteer-extra/blob/master/packages/puppeteer-extra-plugin-stealth/evasions/navigator.hardwareConcurrency/index.js

(() => {

const patchNavigator = (name, value) =>
        utils.replaceProperty(Object.getPrototypeOf(navigator), name, {
          get() {
            return value
          }
        })

      patchNavigator('hardwareConcurrency', opts.hardwareConcurrency || 4)

})();