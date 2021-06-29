function hideOtherElements(xPath) {

    // This implementation was adapted from https://stackoverflow.com/a/44877057/5383169 (RobG)

    var selectedNode = document.evaluate(xPath, document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;
    var currentNode, hiddenNodes = [];

    // Collect the nodes to be hidden.

    do {

        var parentNode = selectedNode.parentNode;

        for (var i = 0, childNodesLength = parentNode.childNodes.length; i < childNodesLength; ++i) {

            currentNode = parentNode.childNodes[i];

            if (currentNode.nodeType == Node.ELEMENT_NODE && currentNode != selectedNode)
                hiddenNodes.push(currentNode);

        }

        selectedNode = parentNode;

    } while (selectedNode.parentNode != null);

    // Hide the collected nodes.

    hiddenNodes.forEach(function (node) {

        node.style.display = 'none';

    });

    return hiddenNodes;

}

function restoreHiddenElements(hiddenNodes) {

    hiddenNodes.forEach(function (node) {

        node.style.display = 'initial';

    });

}