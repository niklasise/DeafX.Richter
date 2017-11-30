import * as React from 'react';
import { Link } from 'react-router-dom'

export default class App extends React.Component<any, any> {

    public render() {
        return <div>
            {this.props.children}
        </div>;
    }

}
