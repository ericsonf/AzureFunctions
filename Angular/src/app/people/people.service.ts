import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Person } from '../person';
import { environment } from '../../environments/environment'
import { take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class PeopleService {

  private readonly API = `${environment.API}`;

  constructor(private http: HttpClient) { }

  list() {
    return this.http.get<Person[]>(this.API + 'ListPeople');
  }

  getById(id) {
    return this.http.get<Person>(`${this.API}GetPerson?id=${id}`)
      .pipe(take(1));
  }

  private create(person) {
    return this.http.post(this.API + 'CreatePerson', JSON.stringify(person), { responseType: 'text' })
      .pipe(take(1));
  }

  private update(person) {
    return this.http.put(this.API + 'EditPerson', JSON.stringify(person), { responseType: 'text' })
      .pipe(take(1));
  }

  save(person) {
    if (person.rowKey)
      return this.update(person);
    else
      return this.create(person);
  }

  remove(id) {
    return this.http.delete(`${this.API}DeletePerson?id=${id}`, { responseType: 'text' })
      .pipe(take(1));
  }
}
