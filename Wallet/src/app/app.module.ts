import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastyModule } from 'ng2-toasty';
import { NgxQRCodeModule} from '@techiediaries/ngx-qrcode';

import { AppComponent } from './components/app/app.component';
import { WalletComponent} from './components/wallet/wallet.component';
import { HomeComponent } from './components/home/home.component';
import { NavigationComponent } from './components/navigation/navigation.component';


@NgModule({
  declarations: [
    AppComponent,
    WalletComponent,
    HomeComponent,
    NavigationComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    NgxQRCodeModule,
    MatInputModule,
    NgbModule.forRoot(),
    ToastyModule.forRoot(),
    RouterModule.forRoot([
      { path: '', redirectTo: '/home' , pathMatch: 'full' },
      { path: 'home', component: HomeComponent },
      { path: 'wallet', component: WalletComponent },
      { path: '**', redirectTo: '/home'}
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
}
