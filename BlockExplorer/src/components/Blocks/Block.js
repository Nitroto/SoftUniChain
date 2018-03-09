import React, {Component} from 'react';
import './Block.sass';
import axios from 'axios/index';
import Moment from 'react-moment';
import {Link} from 'react-router-dom'

class Block extends Component {
    constructor(props) {
        super(props);

        this.state = {
            block: {
                mineBy: {},
                transactions: []
            }
        };
    }

    componentDidMount() {
        axios.get('http://localhost:5000/api/blocks/' + this.props.match.params.id).then(res => {
            const block = res.data;
            this.setState({block});
        })
    }

    render() {
        return (
            <section className="section">
                <div className="container">
                    <h1 className="title">Block #{this.state.block.index}</h1>
                    <div className="columns">
                        <div className="column">
                            <table className="table table is-striped is-fullwidth">
                                <thead>
                                <tr>
                                    <th colSpan="2"><abbr title="Summary"></abbr>Summary</th>
                                </tr>
                                </thead>
                                <tbody>
                                <tr>
                                    <td>Number Of Transactions</td>
                                    <td>{this.state.block.transactions.length}</td>
                                </tr>
                                <tr>
                                    <td>Height</td>
                                    <td>{this.state.block.index}</td>
                                </tr>
                                <tr>
                                    <td>Timestamp</td>
                                    <td>
                                        <Moment format='MMMM Do YYYY, h:mm:ss a'>{this.state.block.createdOn}</Moment>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Relayed By</td>
                                    <td>{this.state.block.mineBy.addressId}</td>
                                </tr>
                                <tr>
                                    <td>Difficulty</td>
                                    <td>{this.state.block.difficulty}</td>
                                </tr>
                                <tr>
                                    <td>Nonce</td>
                                    <td>{this.state.block.nonce}</td>
                                </tr>
                                </tbody>
                            </table>
                        </div>
                        <div className="column">
                            <table className="table is-striped is-fullwidth">
                                <thead>
                                <tr>
                                    <th colSpan="2"><abbr title="Hashes"></abbr>Hashes</th>
                                </tr>
                                </thead>
                                <tbody>
                                <tr>
                                    <td>Data Hash</td>
                                    <td>{this.state.block.blockDataHash}</td>
                                </tr>
                                <tr>
                                    <td>Hash</td>
                                    <td>
                                        <Link to={'/blocks/' + this.state.block.blockHash}>
                                            {this.state.block.blockHash}
                                        </Link>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Previous Block</td>
                                    <td>{this.state.block.previousBlockHash}</td>
                                </tr>
                                <tr>
                                    <td>Next Block(s)</td>
                                    <td>not implemented</td>
                                </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    {this.state.block.transactions.length > 0 &&
                    <h1 className="title">Transactions</h1>
                    }
                    {this.state.block.transactions.map((tx, i) =>
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
                                    <td>{tx.value}</td>
                                </tr>
                                <tr>
                                    <td colSpan="2">Fee:</td>
                                    <td>{tx.fee}</td>
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

export default Block;