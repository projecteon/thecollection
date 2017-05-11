function containsLikeAll (source, target) {
    if (source === null || target === null) return false;
    for(var i=0; i<target.length; i++) {
      if(!source.some(function(item) { return item.indexOf(target[i]) >= 0; } ))
        return false;
    }

    return true;
}