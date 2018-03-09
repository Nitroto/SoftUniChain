import React, {Component} from 'react';
import './Address.sass';
import axios from 'axios/index';
import Currency from 'react-currency-formatter';
import Moment from 'react-moment';
import {Link} from 'react-router-dom';
import QRCode from 'qrcode-react';

class Address extends Component {
    constructor(props) {
        super(props);

        this.state = {
            address: {
                transactions: []
            },
            userTransactions: [],
            validAddress: false
        };
    }

    componentDidMount() {
        axios.get('http://localhost:5000/api/addresses/' + this.props.match.params.id).then(res => {
            const address = res.data;
            this.setState({address});
            const validAddress = true;
            this.setState({validAddress});
        });
        // axios.get('http://localhost:5000/api/addresse/transactions' + this.props.match.params.id).then(res => {
        //     const userTransactions = res.data;
        //     this.setState({userTransactions});
        // });
    }

    render() {
        return (
            <section className="section">
                <div className="container">
                    {!this.state.validAddress &&
                    <h1 className="title">Address not found.</h1>
                    }
                    <table className="table is-striped is-fullwidth">
                        <thead>
                        <tr>
                            <th colSpan="2"><abbr title="Summary"></abbr>Summary</th>
                        </tr>
                        </thead>
                        <tbody>
                        <tr>
                            <td>Address</td>
                            <td><Link to={'/address/' + this.state.address.addressId}>
                                {this.state.address.addressId}
                            </Link>
                            </td>
                        </tr>
                        </tbody>
                    </table>

                    <table className="table is-striped is-fullwidth">
                        <thead>
                        <tr>
                            <th colSpan="2"><abbr title="Transactions"></abbr>Transactions</th>
                        </tr>
                        </thead>
                        <tbody>
                        <tr>
                            <td>No. Transactions</td>
                            <td>{this.state.address.transactions.length}</td>
                        </tr>
                        <tr>
                            <td>Total Received</td>
                            <td>
                                <Currency
                                    quantity={0}
                                    pattern="##,###.00### !"
                                    symbol="SUC"
                                    decimal="."/>
                            </td>
                        </tr>
                        <tr>
                            <td>Final Balance</td>
                            <td>
                                <Currency
                                    quantity={this.state.address.amount / 1000000}
                                    pattern="##,###.00### !"
                                    symbol="SUC"
                                    decimal="."/>
                            </td>
                        </tr>
                        </tbody>
                    </table>
                    <br/>
                    <div className="columns is-centered">
                        <QRCode size={200} value={this.state.address.addressId}/>
                    </div>
                    <br/>
                    {this.state.userTransactions.length > 0 &&
                    <h1 className="title">Transactions</h1>
                    }
                    {this.state.userTransactions.map((tx, i) =>
                        <div key={i}>
                            <table className="table is-striped is-fullwidth">
                                <thead>
                                <tr bgcolor={tx.transferSuccessful ? '#d0f0c0' : '#ffd9e0'}>
                                    <th colSpan="2"><abbr title="Hashes"></abbr>
                                        <Link to={'/transactions/' + tx.transactionHash}>
                                            {tx.transactionHash}
                                        </Link>
                                    </th>
                                    <th><abbr title="Date Created"></abbr>
                                        <Moment format='MMMM Do YYYY, h:mm:ss a'>
                                            {tx.dateCreated}
                                        </Moment>
                                    </th>
                                </tr>
                                </thead>
                                <tbody>
                                <tr>
                                    <td>
                                        <Link to={'/address/' + tx.from.addressId}>
                                            {tx.from.addressId}
                                        </Link>
                                    </td>
                                    <td>
                                    <span className='icon'>
                                        <i className='fas fa-arrow-right successful'></i>
                                    </span>
                                    </td>
                                    <td>
                                        <Link to={'/address/' + tx.to.addressId}>
                                            {tx.to.addressId}
                                        </Link>
                                    </td>
                                </tr>
                                <tr>
                                    <td colSpan="2">Amount:</td>
                                    <td>
                                        <Currency
                                            quantity={tx.value}
                                            pattern="##,### !"
                                            symbol="&mu;SUC"
                                            decimal="."/>
                                    </td>
                                </tr>
                                <tr>
                                    <td colSpan="2">Fee:</td>
                                    <td>
                                        <Currency
                                            quantity={tx.fee}
                                            pattern="##,### !"
                                            symbol="&mu;SUC"
                                            decimal="."/>
                                    </td>
                                </tr>
                                </tbody>
                            </table>
                        </div>
                    )}
                </div>
            </section>
        );
    }
}

export default Address;
