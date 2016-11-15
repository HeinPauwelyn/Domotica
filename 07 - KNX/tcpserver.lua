-- init server handler
if not ready then
  require('copas')
 
  -- list of client sockets
  clients = {}
 
  -- incoming data handler
  function datahandler(sock, data)
    local ip, port
    ip, port = sock:getpeername()
    alert('[server] data from %s:%d - %s', ip, port, data)
   
    
    -- send reply
    if data == 'HELLO' then
      sock:send('HELLO TO YOU! \r\n')
    end
    
    if data == 'stijn' then
      sock:send('muhahaha stijn! \r\n')
    end
    
    -- groepsfunctie aanroepen  
    if(string.match(data, "alex")) then
      alert('alex matched')
      kxnAlex(sock,data)
    end
    
    knxsander(sock,data)
    knxhenk(sock,data)
    groep1(sock,data)
    knxmilan(sock,data)
    knxjordy(sock, data)
    knxSanderVerkaemer(sock,data)
    knxthomas(sock,data)
    knxhein(sock,data)
    
    sock:send('This is no SPAM!/r/n   -- Anonymous/r/n')
    
    -- einde groepsfunctie
    
  end
 -- connection handler
  function connhandler(sock)
    -- enable keep-alive to check for disconnect events
    sock:setoption('keepalive', true)
 
    local ip, port, data, err, id
 
    -- get ip and port from socket
    ip, port = sock:getpeername()
 
    -- client id
    id = string.format('%s:%d', ip, port)
 
    alert('[server] connection from %s', id)
 
    -- save socket reference
    clients[ id ] = sock
 
    -- main reader loop
    while true do
      -- wait for single line of data (until \n, \r is ignored)
      data, err = copas.receive(sock, '*l')
  
    
      -- error while receiving
      if err then
        alert('[server] closed connection from %s:%d', ip, port)
        -- remove socket reference
        clients[ id ] = nil
        return
      end
 
      -- handle data frame
      datahandler(sock, data)
    end
  end
  
   
  -- bind to port 12345
  tcpserver = socket.bind('*', 12345)
 
  -- error while binding, try again later
  if not tcpserver then
    os.sleep(5)
    error('[server] error: cannot bind')
  end
  
  -- start groepfuncties
  function knxhein(sock,data)
    -- send reply
    if data == 'BLIJ' then
      sock:send('framblij /r/n')
    end
    
    if data == 'BOOS' then
      sock:send('framboos /r/n')
    end
    
    sock:send(data + ' /r/n')
  end
 
  
  
  function knxhenk(sock,data)
    -- send reply
    if data == 'HENK' then
      sock:send('HELLO TO YOU HENK! \r\n')
    end
    
       -- hbt  send write 1 to group
    if data == 'ON' then
      sock:send('write on group\n')
    	grp.write('4/0/0', true)
    end
    
    if data == 'OFF' then
      grp.write('4/0/0', false)
      sock:send('write on group\n')
    end
      
      if data == 'temperatuur' then
        sock:send('read on group\n')
        grp.read('1/0/0')
      end
  end
  
    -- start groepfuncties
  function knxsander(sock,data)
    -- send reply
    if data == 'SANDER' then
      sock:send('HELLO TO YOU stijn VERVAEKE! \r\n')
    end
    
       -- hbt  send write 1 to group
    if data == 'ON' then
      sock:send('write on group\n')
    	grp.write('4/0/0', true)
    end
    
    if data == 'OFF' then
      grp.write('4/0/0', false)
      sock:send('write on group\n')
    end
      
      if data == 'temperatuur' then
        sock:send('read on group\n')
        grp.read('1/0/0')
      end
  end
  
  function knxmilan(sock,data)
    -- send reply
    if data == 'MILAN' then
      sock:send('WELKOM MILAN \r \n')
    end
  end
  
  function knxjordy(sock,data)
    -- send reply
    if data == 'JORDY' then
      sock:send('WELKOM JORDY \r \n')
    end
  end
  
      -- start mijn functie
  function knxthomas(sock, data)
      -- send reply
      if data == 'thomas' then
      sock:send('Hallo Thomas, welkom op de knx. /r/n')
      end
  end
  
    function groep1(sock,data)
      
    end
  
  function knxAlex(sock,data)
      
  end
  
  function knxSanderVerkaemer(sock,data)
    -- send reply
    if data == 'VERKAEMER' then
      sock:send('HELLO stijn! \r\n')
    end
    
       -- hbt  send write 1 to group
    if data == '15-ON' then
      sock:send('write on group\n')
    	grp.write('4/0/0', true)
    end
    
    if data == '15-OFF' then
      grp.write('4/0/0', false)
      sock:send('write on group\n')
    end
      
  end
    -- einde groepsfuncties

  -- set server connection handler
  copas.addserver(tcpserver, connhandler)
 
  -- create local udp server on port 23456
  udpserver = socket.udp()
  udpserver:setsockname('127.0.0.1', 23456)
 udpserver:settimeout(0.1)
 
  ready = true
end
 
-- perform server tasks for one second (5 x (0.1 + 0.1))
for i = 1, 5 do
  message = udpserver:receive()
 
  -- got message from udp, send to all active clients
if message then
   for id, sock in pairs(clients) do
      sock:send(message .. '\r\n')
   end
 end
 
  copas.step(0.1)
end