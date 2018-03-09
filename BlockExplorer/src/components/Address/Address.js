import React, {Component} from 'react';
import './Address.sass';
import axios from 'axios/index';
import Moment from 'react-moment';
import {Link} from 'react-router-dom'

class Address extends Component {
    constructor(props) {
        super(props);

        this.state = {
            address: {}
        };
    }

    componentDidMount() {
        axios.get('http://localhost:5000/api/addresses/' + this.props.match.params.id).then(res => {
            const address = res.data;
            this.setState({address});
        })
    }

    render() {
        console.log(this.state.address);
        return (
            <section className="section">

            </section>
        );
    }
}

export default Address;
