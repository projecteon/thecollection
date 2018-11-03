const path = require('path');
const webpack = require('webpack');
const ExtractTextPlugin = require('extract-text-webpack-plugin');
const merge = require('webpack-merge');
const webpackCommon = require('./webpack.config.common');

module.exports = (env) => {
    const isDevBuild = !(env && env.prod);
    const extractCSS = new ExtractTextPlugin('vendor.css');

    const sharedConfig = {
        stats: { modules: false },
        resolve: { extensions: [ '.js' ] },
        module: {
            rules: [
                { test: /\.(png|woff|woff2|eot|ttf|svg)(\?|$)/, use: 'url-loader?limit=100000' },
                { test: /\.css(\?|$)/, use: extractCSS.extract({ use: isDevBuild ? 'css-loader' : 'css-loader?minimize' }) }
            ]
        },
        entry: {
            vendor: [
                'bootstrap',
                'bootstrap/dist/css/bootstrap.css',
                'domain-task',
                'c3',
                'c3/c3.css',
                'event-source-polyfill',
                'history',
                'moment',
                'popper.js',
                'react',
                'react-dom',
                'react-router',
                'react-redux',
                'redux',
                'redux-thunk',
                'react-router-redux',
                'jquery',
            ],
        },
        output: {
            publicPath: '/dist/',
            filename: '[name].js',
            library: '[name]_[hash]',
            path: path.join(__dirname, 'wwwroot', 'dist'),
        },
        plugins: [
            new webpack.ProvidePlugin({ $: 'jquery', jQuery: 'jquery', 'window.jQuery': 'jquery', Popper: ['popper.js', 'default'] }), // Maps these identifiers to the jQuery package (because Bootstrap expects it to be a global variable)
            new webpack.NormalModuleReplacementPlugin(/\/iconv-loader$/, require.resolve('node-noop')), // Workaround for https://github.com/andris9/encoding/issues/16
            new webpack.DefinePlugin({
                'process.env.NODE_ENV': isDevBuild ? '"development"' : '"production"'
            }),
            extractCSS,
            new webpack.DllPlugin({
              path: path.join(__dirname, 'wwwroot', 'dist', '[name]-manifest.json'),
              name: '[name]_[hash]'
          })
        ]
    };

    const clientBundleConfig = isDevBuild ? sharedConfig : merge(sharedConfig, webpackCommon.loadProduction());

    return [clientBundleConfig];
};
