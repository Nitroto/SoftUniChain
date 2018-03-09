import React, {Component} from 'react';
import './App.css';
import {Route} from 'react-router-dom';
import Header from './components/Header/Header';
import Footer from './components/Footer/Footer';
import Home from './components/Home/Home';
import Blocks from './components/Blocks/Blocks';
import Block from './components/Blocks/Block';
import Transaction from './components/Transaction/Transaction';
import Address from './components/Address/Address';

class App extends Component {
    render() {
        return (
            <div className="App">
                <Header/>

                <Route exact={true} path="/" component={Home}/>
                <Route exact={true} path="/blocks" component={Blocks}/>
                <Route exact={true} path="/blocks/:id" component={Block}/>
                <Route exact={true} path="/transactions/:id" component={Transaction}/>
                <Route exact={true} path="/address/:id" component={Address}/>

                <Footer/>
            </div>
        );
    }
}

export default App;
