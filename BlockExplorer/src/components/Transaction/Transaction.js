import React, {Component} from 'react';
import './Transaction.sass';
import axios from 'axios/index';
import Moment from 'react-moment';
import {Link} from 'react-router-dom';

class Transaction extends Component {
    constructor(props) {
        super(props);

        this.state = {
            transaction: {}
        };
    }

    componentDidMount() {
        axios.get('http://localhost:5000/api/transactions/' + this.props.match.params.id).then(res => {
            const transaction = res.data;
            this.setState({transaction});
        })
    }

    render() {
        return (
            <section className="section">
                <div className="container">
                    <table className="table is-striped is-fullwidth">
                        <thead>
                        <tr bgcolor={this.state.transaction.transferSuccessful ? '#d0f0c0' : '#ffd9e0'}>
                            <th colSpan="2">
                                <abbr title="Hashes"></abbr>
                                <Link to={'/transactions/' + this.state.transaction.transactionHash}>
                                    {this.state.transaction.transactionHash}
                                </Link>
                            </th>
                            <th>
                                <abbr title="Date Created"></abbr>
                                <Moment format='MMMM Do YYYY, h:mm:ss a'>
                                    {this.state.transaction.dateCreated}
                                </Moment>
                            </th>
                        </tr>
                        </thead>
                        <tbody>
                        <tr>
                            <td>
                                <Link to={'/address/' + this.state.transaction.from}>
                                    {this.state.transaction.from}
                                </Link>
                            </td>
                            <td>
                                    <span className='icon'>
                                        <i className='fas fa-arrow-right successful'></i>
                                    </span>
                            </td>
                            <td>
                                <Link to={'/address/' + this.state.transaction.to}>
                                    {this.state.transaction.to}
                                </Link>
                            </td>
                        </tr>
                        <tr>
                            <td colSpan="2">Amount:</td>
                            <td>{this.state.transaction.value}</td>
                        </tr>
                        <tr>
                            <td colSpan="2">Fee:</td>
                            <td>{this.state.transaction.fee}</td>
                        </tr>
                        <tr>
                            <td colSpan="2">Block:</td>
                            <td>
                                <Link to={'/blocks/' + this.state.transaction.minedInBlockIndex}>
                                    {this.state.transaction.minedInBlockIndex}
                                </Link>
                            </td>
                        </tr>
                        </tbody>
                    </table>
                </div>
            </section>
        );
    }
}

export default Transaction;
