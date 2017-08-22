const webpack = require('webpack');
const CheckerPlugin = require('awesome-typescript-loader').CheckerPlugin;
const ExtractTextPlugin = require('extract-text-webpack-plugin');

exports.loadUrl = function () {
  return {
    module: {
      rules: [
        {
          test: /\.(png|jpg|jpeg|gif|svg)$/,
          use: [
            {
              loader: 'url-loader',
              options: {
                limit: 25000
              }
            }
          ]
        }
      ]
    }
  }
};

exports.loadCSS = function (minimize) {
  let options = minimize ? {minimize: true} : {};
  return {
    module: {
      rules: [
        {
          test: /\.css$/,
          use: [
            {loader: 'style-loader'},
            {loader: 'css-loader', options: options}
          ]
        }
      ]
    }
  };
}

exports.loadSass = function (minimize, filename) {
  let options = minimize ? {minimize: true, sourceMap: true} : {sourceMap: true};
  let extractCSS = new ExtractTextPlugin({
    filename: filename
  });
  return {
    module: {
      rules: [
        {
          test: /\.(css|scss)$/,
          use: extractCSS.extract({
            fallback: 'style-loader',
            use: [{
              loader: 'css-loader',
              options: options
            },
            {
              loader: 'sass-loader',
              options: options
            }]
          })
        }
      ]
    },
    plugins: [extractCSS]
  };
}

exports.extractBundle = function(options) {
  return {
    // Define an entry point needed for splitting.
    plugins: [
      // Extract bundle and manifest files. Manifest is
      // needed for reliable caching.
      new webpack.optimize.CommonsChunkPlugin({
        names: [options.name, 'manifest'],
        minChunks: Infinity
      })
    ]
  };
}

exports.loadTypescript = function() {
  var rules = [
    { loader: 'babel-loader', options: { cacheDirectory: true } },
    { loader: 'awesome-typescript-loader', options: {silent: true}}
  ];

  return {
    module: {
      rules: [
        {
          test: /\.ts(x?)$/,
          include: /ClientApp/,
          exclude: 'node_modules',
          use: rules
        }
      ]
    }
  };
};

exports.tsLint = function() {
  return {
    module: {
      rules: {
        test: /\.ts(x?)$/,
        enfore: 'pre',
        loader: 'tslint-loader'
      }
    }
  }
}

exports.CheckerPlugin = function() {
  return {
    plugins: [new CheckerPlugin()]
  }
}
