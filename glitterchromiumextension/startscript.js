
function checkEditor() {
    // check for content editor
    
}

function getCurrentItemInformation() {
    
    var itemId, itemLanguage, itemVersion, editor;

    if(document.URL.indexOf("sitecore/shell/Applications/Content%20Editor.aspx") > -1) {
        editor = "contenteditor";
    } else {
        editor = "experienceeditor";
    }

    switch(editor) {
        case "experienceeditor":
            var itemIdElement = document.getElementById("scItemID");
            var itemLanguageElement = document.getElementById("scLanguage");
            var itemVersionElement = document.getElementById("scVersion");
            
            if (itemIdElement != null && itemLanguageElement != null && itemVersionElement != null) {
                ItemId = itemVersion.value;
                itemLanguage = itemLanguageElement.value; 
                itemVersion = itemVersionElement.value;
            }
            
            // if nothing was found, try to get the data from the querystring
            let params = new URLSearchParams(window.location.search);
            itemId = params.get("sc_itemid");
            itemLanguage = params.get("sc_lang");
            itemVersion = params.get("sc_version");

            return {itemId, itemLanguage, itemVersion};
            
        case "contenteditor":
            // Experience editor
            var itemInfo = document.getElementById("__CurrentItem").value
            //"sitecore://master/{6702D14A-E01D-4E32-B722-A2B30AE9BD1D}?lang=en&ver=1"
            itemId = itemInfo.substring(itemInfo.indexOf("{"), itemInfo.indexOf("}") + 1);
            itemLanguage = itemInfo.substring(itemInfo.indexOf("lang=") + 5, itemInfo.indexOf("&ver="));
            itemVersion = itemInfo.substring(itemInfo.indexOf("ver=") + 4);
            return {itemId, itemLanguage, itemVersion};
    }

    function queryApi(itemId, itemLanguage, itemVersion) {
        fetch('https://jsonplaceholder.typicode.com/posts').then(function (response) {
	        // The API call was successful!
	        console.log('success!', response);
        }).catch(function (err) {
	        // There was an error
	        console.warn('Something went wrong.', err);
        });
    }

}

chrome.tabs.query({
    active: true,
    lastFocusedWindow: true
}, function(tabs) {
    var tab = tabs[0];
    chrome.scripting.executeScript({
        target : {tabId : tab.id},
        func : getCurrentItemInformation
      })
      .then(injectionResults => {
        for (const frameResult of injectionResults) {
          const {frameId, result} = frameResult;
          updateExtensionInterface(result.itemId, result.itemLanguage, result.itemVersion);
        }
      });
});


function updateExtensionInterface(itemId, itemLanguage, itemVersion) {
    var historyElement = document.getElementById("history");
    historyElement.innerHTML = "ItemId: " + itemId + "<br/>Language: " + itemLanguage + "<br/>" + "<br/>Version: " + itemVersion + "<br/>";
}


