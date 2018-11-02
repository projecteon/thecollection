export function fetchJson<T>(url: string) {
  return fetch(url, { credentials: 'same-origin' })
    .then(res => {
      if (res.status === 500) {
        throw new Error('Server error.');
      }

      if (res.status === 400) {
        throw new Error(`Bad request: ${res.text}`);
      }

      return res.json() as Promise<T>;
    })
    .then(result => result);
}

export function putJson<T>(url: string, id: string, payload: T) {
  return fetch(`${url}${id}`, {
    method: 'PUT',
    // tslint:disable-next-line:object-literal-sort-keys
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json; charset=utf-8',
      'dataType': 'json',
    },
    credentials: 'same-origin',
    body: JSON.stringify(payload),
  })
  .then(res => {
    if (res.status === 500) {
      throw new Error('Server error.');
    }

    if (res.status === 400) {
      throw new Error(`Bad request: ${res.text}`);
    }

    return res.json() as Promise<T>;
  })
  .then(result => result);
}

export function postJson<T>(url: string, payload: T) {
  return fetch(url, {
    method: 'POST',
    // tslint:disable-next-line:object-literal-sort-keys
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json; charset=utf-8',
      'dataType': 'json',
    },
    credentials: 'same-origin',
    body: JSON.stringify(payload),
  })
  .then(res => {
    if (res.status === 500) {
      throw new Error('Server error.');
    }

    if (res.status === 400) {
      throw new Error(`Bad request: ${res.text}`);
    }

    return res.json() as Promise<T>;
  })
  .then(result => result);
}

export function postFiles(url: string, payload: File[]) {
  const data = new FormData();
  payload.forEach((file, index) => { data.append(`files`, file, file.name); }); // creates an array files with all files on formdata

  return fetch(url, {
    method: 'POST',
    // tslint:disable-next-line:object-literal-sort-keys
    body: data,
    credentials: 'same-origin',
  })
  .then(result => result);
}

export function convertFileListToArray(filelist: FileList): File[] {
  const data: File[] = [];
  // tslint:disable-next-line:prefer-for-of
  for (let i = 0; i < filelist.length; i++) {
    data.push(filelist[i]);
  }

  return data;
}
