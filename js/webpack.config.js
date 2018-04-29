const path = require('path');
const webpack = require('webpack');
const CheckerPlugin = require('awesome-typescript-loader').CheckerPlugin;
const UglifyJSPlugin = require('uglifyjs-webpack-plugin');

module.exports = (env) => {
    const isDevBuild = !(env && env.prod);
    const clientBundleOutputDir = './wwwroot/dist/js';

    // Configuration in common to both client-side and server-side bundles
    return {
        entry: {
            app: ["whatwg-fetch", "index.tsx"],
        },
        resolve: { 
            extensions: ['.js', '.jsx', '.ts', '.tsx'],
            modules: [path.resolve(__dirname, "src"), "node_modules"]
        },
        output: {
            path: path.join(__dirname, clientBundleOutputDir),
            filename: '[name].js',
            publicPath: 'dist/js/'
        },
        devServer: {
            contentBase: path.join(__dirname, "wwwroot"),
            compress: true,
            port: 9000,
            historyApiFallback: true
        },
        module: {
            rules: [
                { test: /\.tsx?$/, include: /src/, use: ['babel-loader', 'awesome-typescript-loader?silent=true'] }
            ]
        },
        plugins: [
            new CheckerPlugin()
        ].concat(isDevBuild ?
            [
                // new webpack.DllReferencePlugin({
                //     context: __dirname,
                //     manifest: require('./wwwroot/dist/vendor-manifest.json')
                // }),
                // Plugins that apply in development builds only
                new webpack.SourceMapDevToolPlugin({
                    filename: '[file].map', // Remove this line if you prefer inline source maps
                    moduleFilenameTemplate: path.relative(clientBundleOutputDir, '[resourcePath]') // Point sourcemap entries to the original file locations on disk
                })]
            : [
                // Plugins that apply in production builds only
                new UglifyJSPlugin()
            ])
    };

};
