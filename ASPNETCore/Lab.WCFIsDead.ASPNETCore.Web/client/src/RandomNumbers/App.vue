<template>
  <div id="app">
    <HelloWorld msg="Random numbers"/>
  </div>
</template>

<script lang="ts">
    import { Component, Vue } from 'vue-property-decorator';
    import HelloWorld from '../components/HelloWorld.vue';
    import * as signalR from '@aspnet/signalr';
    import { Guid } from 'guid-typescript';

@Component({
  components: {
    HelloWorld,
  },
})
export default class App extends Vue {

    created () {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("/hubs/randomNumbers")
            .configureLogging(signalR.LogLevel.Information)
            .build();

       

        connection.start().then(() => {
            connection.stream("GetRandomNumbers", Guid.create().toString(), 10, 1000)
            .subscribe(new RandomNumberLogger());

            connection.stream("GetRandomNumbers", Guid.create().toString(), 5, 2000)
            .subscribe(new RandomNumberLogger());
        });

        
    }

    private receiveRandomNumber(requestId: string, randomNumber: number) : void{
        console.log(requestId + ": " + randomNumber);
    }

}

    class RandomNumberLogger implements signalR.IStreamSubscriber<RandomNumberResult>{

        next = (item: RandomNumberResult) => { console.log(item.requestId + ": " + item.randomNumber); }
        complete = () => { }
        error = (err: any) => { }

    }

    class RandomNumberResult {

        requestId: string = "";
        randomNumber: number = 0;

    }
</script>

<style>
#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
