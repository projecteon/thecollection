const path = require('path');
const merge = require('webpack-merge');
const webpackCommon = require('./webpack.config.common');

module.exports = (env) => {
    const isDevBuild = !(env && env.prod);
    const clientBundleOutputDir = './wwwroot/dist';

    // Configuration in common to both client-side and server-side bundles
    const sharedConfig = () => ({
        stats: { modules: false },
        resolve: { extensions: ['.js', '.jsx', '.ts', '.tsx'] },
        entry: { 'main-client': './ClientApp/boot-client.tsx' },
        output: {
            filename: '[name].js',
            publicPath: '/dist/', // Webpack dev middleware, if enabled, handles requests for this URL prefix
            path: path.join(__dirname, clientBundleOutputDir)
        },
    });

    // Configuration for client-side bundle suitable for running in browsers
    const clientBundleConfig = merge(
      sharedConfig(),
      webpackCommon.loadCheckerPlugin(),
      webpackCommon.loadTypescript(),
      webpackCommon.loadUrlImages(),
      webpackCommon.loadUrlFonts(),
      webpackCommon.loadSass(isDevBuild, 'site.css'),
      webpackCommon.loadDlls('./wwwroot/dist/vendor-manifest.json'),
      isDevBuild ? webpackCommon.loadDevelopment(clientBundleOutputDir) : webpackCommon.loadProduction()
    );

    return [clientBundleConfig];
};
