// Get item information from Sitecore
function getCurrentItemInformation() {
    
    var itemId, itemLanguage, itemVersion, editor;
    
    var determineSitecoreEditor = function() {
        if(document.URL.indexOf("sitecore/shell/Applications/Content%20Editor.aspx") > -1) {
            editor = "contenteditor";
        } else {
            editor = "experienceeditor";
        }
        return editor;
    }


    var editor = determineSitecoreEditor();

    switch(editor) {
        case "experienceeditor":
            var itemIdElement = document.getElementById("scItemID");
            var itemLanguageElement = document.getElementById("scLanguage");
            var itemVersionElement = document.getElementById("scVersion");
            
            if (itemIdElement != null && itemLanguageElement != null && itemVersionElement != null) {
                itemId = itemIdElement.value;
                itemLanguage = itemLanguageElement.value; 
                itemVersion = itemVersionElement.value;
            }
            
            // if nothing was found, try to get the data from the querystring
            if (itemId == null || itemLanguage == null || itemVersion == null) { 
                let params = new URLSearchParams(window.location.search);
                itemId = params.get("sc_itemid");
                itemLanguage = params.get("sc_lang");
                itemVersion = params.get("sc_version");
            }
            
            
            // if itemVersion was not found found, try to get the itemversion from the ribbon :S
            if (itemVersion == null) {
                var element = document.getElementById('scWebEditRibbon').contentWindow.document.querySelectorAll('[data-sc-id="PageEditBar"]');
                if (element && element.length > 0) {
                    itemVersion = element[0].getAttribute("data-sc-version");
                }
            }

            return {itemId, itemLanguage, itemVersion};
            
        case "contenteditor":
            // Experience editor
            var itemInfo = document.getElementById("__CurrentItem").value
            itemId = itemInfo.substring(itemInfo.indexOf("{"), itemInfo.indexOf("}") + 1);
            itemLanguage = itemInfo.substring(itemInfo.indexOf("lang=") + 5, itemInfo.indexOf("&ver="));
            itemVersion = itemInfo.substring(itemInfo.indexOf("ver=") + 4);
            return {itemId, itemLanguage, itemVersion}
    }

    
}

// extension startup script
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


// update the extension gui with item information + glitter data
async function updateExtensionInterface(itemId, itemLanguage, itemVersion) {
    var historyElement = document.getElementById("history");
    var apiData = await fetchApiData(itemId, itemLanguage, itemVersion);
    historyElement.innerHTML = "ItemId: " + itemId + "<br/>Language: " + itemLanguage + "<br/>" + "<br/>Version: " + itemVersion + "<br/>" + apiData;
}


// function to fetch data from the glitter API
async function fetchApiData(itemId, itemLanguage, itemVersion) {
    let cleaneditemId = itemId.replace("{", "").replace("}", "");
    let url = "https://glitterbucket.localhost/item/" + cleaneditemId + "/version/" + itemVersion;
    let response = await fetch(url);
    let historydata = await response.json();
    return historydata[0].title;
}